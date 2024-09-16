using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Dtos;
using MuevemeApi.Data;
using MuevemeApi.Models;

namespace MuevemeApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VehicleController : ControllerBase
{
    private readonly DataContextDapper _dapper;

    public VehicleController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }
    
    [HttpGet]
    public IEnumerable<Vehicle> GetVehicles()
    {
        string userId = User.FindFirst("userId")?.Value + "";

        return _dapper.FindMany<Vehicle>($"SELECT * FROM [MuevemeSchema].[Vehicle] WHERE UserId = {userId}");
    }

    [HttpPost]
    public IActionResult AddVehicle(VehicleDto vehicle) 
    {
        string userId = User.FindFirst("userId")?.Value + "";

        string sqlAddVehicle = @$"
            INSERT INTO [MuevemeSchema].[Vehicle] (Plates, UserId, Model, Color, Year)
            Values ('{vehicle.Plates}', {userId}, '{vehicle.Model}', '{vehicle.Color}', {vehicle.Year})
        ";

        if(!_dapper.ExecuteSql(sqlAddVehicle)) 
        {
            throw new Exception("Failed to add vehicle");
        }

        return Ok();
    }  

    [HttpPut("{vehicleId}")]
    public IActionResult UpdateVehicle(int vehicleId, VehicleDto vehicle)
    {
        string sqlUpdateVehicle = @$"
            UPDATE [MuevemeSchema].[Vehicle]
            SET 
                Plates = '{vehicle.Plates}', 
                Model = '{vehicle.Model}', 
                Color = '{vehicle.Color}', 
                Year = {vehicle.Year}
            WHERE Id = '{vehicleId}'
        ";

        if(!_dapper.ExecuteSql(sqlUpdateVehicle))
        {
            throw new Exception("Failed to update vehicle");
        }

        return Ok();
    }

    [HttpDelete("{vehicleId}")]
    public IActionResult DeleteVehicle(string vehicleId)
    {
        return Ok();
    }
}