using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;
using MuevemeApi.Dtos;
using MuevemeApi.Models;

namespace MuevemeApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TravelController : ControllerBase
{
    private readonly DataContextDapper _dapper;

    public TravelController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<TravelGetDto> GetTravels()
    {
        IEnumerable<TravelGetDto> travels = _dapper.FindMany<TravelGetDto>("SELECT * FROM [MuevemeSchema].[Travel]");
        IEnumerable<DayTravelDto> days = _dapper.FindMany<DayTravelDto>("SELECT D.*, DT.TravelId FROM [MuevemeSchema].[DayTravel] AS DT INNER JOIN [MuevemeSchema].[Day] AS D ON D.Id = DT.DayId");
    
        foreach (var travel in travels)
        {
            travel.ActiveDays = days.Where(d => d.TravelId == travel.Id);
        }

        return travels;
    }

    [HttpPost]
    public IActionResult AddTravel(TravelCreateDto travel)
    {
        string userId = User.FindFirst("userId")?.Value + "";

        string sqlAddTravel = @$"
            INSERT INTO [MuevemeSchema].[Travel] (UserId, VehicleId, StartTime, EndTime, FromCoordinates, ToCoordinates, Price)
            VALUES ({userId}, '{travel.VehicleId}', '{travel.StartTime}', '{travel.EndTime}', '{travel.FromCoordinates}', '{travel.ToCoordinates}', {travel.Price})
        ";

        if(!_dapper.ExecuteSql(sqlAddTravel))
        {
            throw new Exception("Failed to add travel");
        }

        string sqlAddDayTravel = "INSERT INTO [MuevemeSchema].[DayTravel] (TravelId, DayId) VALUES\n";

        int index = 1;

        IEnumerable<int> days = [];
        int travelId = _dapper.FindOne<int>("SELECT MAX(Id) FROM [MuevemeSchema].[Travel]");

        if(travel.PresetMode == 1 && travel.ActiveDay.Count() > 0)
        {
            days = travel.ActiveDay;
        } 
        else 
        {
            days = _dapper.FindMany<int>($"SELECT DayId FROM [MuevemeSchema].[DayScheduleModes] WHERE TravelScheduleModeId = {travel.PresetMode}");
        }

        foreach (var day in days)
        {
            if(index == days.Count())
            {
                sqlAddDayTravel += $"({travelId}, {day})";
            }
            else 
            {
                sqlAddDayTravel += $"({travelId}, {day}),";
            }
            index ++;
        }

        if(!_dapper.ExecuteSql(sqlAddDayTravel))
        {
            throw new Exception("Failed to add travel");
        }

        return Ok();
    }

    [HttpPut("{travelId}")]
    public IActionResult UpdateTravel(int travelId, TravelUpdateDto travel)
    {
        string sqlDeleteActiveDays = $"DELETE FROM [MuevemeSchema].[DayTravel] WHERE TravelId = {travelId}";

        if(!_dapper.ExecuteSql(sqlDeleteActiveDays))
        {
            throw new Exception("Failed to update travel");
        }

        string sqlUpdateTravel = @$"
            UPDATE [MuevemeSchema].[Travel]
            SET 
                VehicleId = '{travel.VehicleId}', 
                StartTime = '{travel.StartTime}', 
                EndTime = '{travel.EndTime}', 
                FromCoordinates = '{travel.FromCoordinates}', 
                ToCoordinates = '{travel.ToCoordinates}', 
                Price = {travel.Price}
            WHERE Id = {travelId}
        ";

        if(!_dapper.ExecuteSql(sqlUpdateTravel))
        {
            throw new Exception("Failed to update travel");
        }

        string sqlAddDayTravel = "INSERT INTO [MuevemeSchema].[DayTravel] (TravelId, DayId) VALUES\n";

        int index = 1;

        IEnumerable<int> days = [];

        if(travel.PresetMode == 1 && travel.ActiveDay.Count() > 0)
        {
            days = travel.ActiveDay;
        } 
        else 
        {
            days = _dapper.FindMany<int>($"SELECT DayId FROM [MuevemeSchema].[DayScheduleModes] WHERE TravelScheduleModeId = {travel.PresetMode}");
        }

        foreach (var day in days)
        {
            if(index == days.Count())
            {
                sqlAddDayTravel += $"({travelId}, {day})";
            }
            else 
            {
                sqlAddDayTravel += $"({travelId}, {day}),";
            }
            index ++;
        }

        if(!_dapper.ExecuteSql(sqlAddDayTravel))
        {
            throw new Exception("Failed to update travel");
        }

        return Ok();
    }
}