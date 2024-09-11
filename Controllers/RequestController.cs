using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;

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
}