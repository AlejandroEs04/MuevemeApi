using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;
using MuevemeApi.Models;

namespace MuevemeApi.Controllers;

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

    public IActionResult AddRequest()
    {
        

        return Ok();
    }
}