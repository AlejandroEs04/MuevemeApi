using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;
using MuevemeApi.Models;

namespace MuevemeApi.Controllers;

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
        return _dapper.FindMany<Vehicle>("SELECT * FROM [MuevemeSchema].[Vehicle]");
    }

    [HttpPost]
    public IActionResult AddVehicle(Vehicle vehicle) 
    {
        string sqlAddVehicle = @$"
            INSERT INTO [MuevemeSchema].[Vehicle] (Id, UserId, Model, Color, Year)
            Values ('{vehicle.Id}', {vehicle.UserId}, '{vehicle.Model}', '{vehicle.Color}', {vehicle.Year})
        ";

        if(!_dapper.ExecuteSql(sqlAddVehicle)) 
        {
            throw new Exception("Failed to add vehicle");
        }

        return Ok();
    }  

    [HttpPut("{vehicleId}")]
    public IActionResult UpdateVehicle(string vehicleId, Vehicle vehicle)
    {
        string sqlUpdateVehicle = @$"
            UPDATE [MuevemeSchema].[Vehicle]
            SET 
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