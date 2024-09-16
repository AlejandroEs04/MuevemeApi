using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;
using MuevemeApi.Dtos;
using MuevemeApi.Models;
using MuevemeApi.Utils;

namespace MuevemeApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly AuthHelper _authHelper;

    public AuthController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        _authHelper = new AuthHelper(config);
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login(UserLoginDto userLogin)
    {
        string existsUser = @$"
            SELECT * FROM [MuevemeSchema].[User]
            WHERE UserName = '{userLogin.UserName}'
        ";

        User user = new User();

        try
        {
            user = _dapper.FindOne<User>(existsUser);

            byte[] passwordHash = _authHelper.GetPasswordHash(userLogin.Password, user.PasswordSalt);
            
            for(int i = 0; i < passwordHash.Length; i++)
            {
                if(passwordHash[i] != user.PasswordHash[i])
                {
                    return StatusCode(401, "Incorrect Password");
                }
            }
        }
        catch (System.InvalidOperationException)
        {
            return StatusCode(404, "User does not exists");
        }

        return Ok(new Dictionary<string, string> {
            {"token", _authHelper.CreateToken(user.Id)}
        });
    }

    [HttpGet("RefreshToken")]
    public IActionResult RefreshToken()
    {
        string userId = User.FindFirst("userId")?.Value + "";

        string sqlGetUserId = @$"
            SELECT Id FROM [MuevemeSchema].[User]
            WHERE Id = {userId}
        ";

        int userIdFromDB = _dapper.FindOne<int>(sqlGetUserId);

        return Ok(new Dictionary<string, string> {
            {"token", _authHelper.CreateToken(userIdFromDB)}
        });

    }
}
