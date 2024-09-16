using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;
using MuevemeApi.Dtos;
using MuevemeApi.Models;

namespace MuevemeApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private readonly DataContextDapper _dapper;

    public RequestController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet]
    public IEnumerable<Request> GetRequests()
    {
        return _dapper.FindMany<Request>("SELECT * FROM [MuevemeSchema].[Request]");
    }

    [HttpPost]
    public IActionResult AddRequest(RequestCreateDto request)
    {
        string userId = User.FindFirst("userId")?.Value + "";

        string sqlAddRequest = @$"
            INSERT INTO [MuevemeSchema].[Request] (TravelId, UserId, Status, Active, Stop, Date)
            VALUES ({request.TravelId}, {userId}, 0, 1, '{request.Stop}', GETDATE())
        ";

        Console.WriteLine(sqlAddRequest);

        if(!_dapper.ExecuteSql(sqlAddRequest))
        {
            throw new Exception("Failed to create request");
        }

        int requestId = _dapper.FindOne<int>("SELECT MAX(Id) FROM [MuevemeSchema].[Request]");

        return Ok(new Dictionary<string, int> {
            {"request_id", requestId}
        });
    }
}