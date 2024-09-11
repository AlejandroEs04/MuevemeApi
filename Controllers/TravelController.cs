using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;
using MuevemeApi.Dtos;
using MuevemeApi.Models;

namespace MuevemeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TravelController : ControllerBase
{
    private readonly DataContextDapper _dapper;

    public TravelController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet]
    public IEnumerable<Travel> GetTravels()
    {
        return _dapper.FindMany<Travel>("SELECT * FROM [MuevemeSchema].[Travel]");
    }

    [HttpPost]
    public IActionResult AddTravel(TravelCreateDto travel)
    {
        string sqlAddTravel = @$"
            INSERT INTO [MuevemeSchema].[Travel] (UserId, VehicleId, StartTime, EndTime, FromCoordinates, ToCoordinates, Price)
            VALUES ({travel.UserId}, '{travel.VehicleId}', '{travel.StartTime}', '{travel.EndTime}', '{travel.FromCoordinates}', '{travel.ToCoordinates}', {travel.Price})
        ";

        if(!_dapper.ExecuteSql(sqlAddTravel))
        {
            throw new Exception("Failed to add travel");
        }

        return Ok();
    }

    [HttpPut("{travelId}")]
    public IActionResult UpdateTravel(int travelId, TravelUpdateDto travel)
    {
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

        return Ok();
    }
}