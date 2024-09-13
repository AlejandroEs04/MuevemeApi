using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;
using MuevemeApi.Dtos;

namespace MuevemeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContextDapper _dapper;

    public AuthController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpPost]
    public IActionResult Login(UserLoginDto userLogin)
    {
        string existsUser = @$"
            SELECT * FROM [Mueveme]
        ";

        return Ok();
    }
}
