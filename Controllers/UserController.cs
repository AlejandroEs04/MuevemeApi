using System.Data;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MuevemeApi.Data;
using MuevemeApi.Dtos;
using MuevemeApi.Models;
using MuevemeApi.Utils;

namespace MuevemeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly AuthHelper _authHelper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        _authHelper = new AuthHelper(config);
    }

    [HttpGet]
    public IEnumerable<UserGetDto> GetUsers()
    {
        string sqlGetUsers = @"
            SELECT 
                Id, Name, LastName, Email, PhoneNumber, UserName, PorfileImageUrl
            FROM [MuevemeSchema].[User]
        ";
        return _dapper.FindMany<UserGetDto>(sqlGetUsers);
    }

    [HttpPost]
    public IActionResult AddUser(UserCreateDto user)
    {
        if(user.Password != user.RepeatPassword) {
            throw new Exception("Passwords do not match");
        }

        string sqlExistsUser = @$"
            SELECT * FROM [MuevemeSchema].[User]
            WHERE Email = '{user.Email}' OR 
            PhoneNumber = '{user.PhoneNumber}' OR 
            UserName = '{user.UserName}' 
        ";

        IEnumerable<User> existsUser = _dapper.FindMany<User>(sqlExistsUser); 

        if(existsUser.Count() > 0) 
        {
            throw new Exception("Another account already exists with this email, phone number or user name");
        }

        byte[] passwordSalt = new byte[129/8];
        using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, passwordSalt);

        string sqlAddUser = @$"
            INSERT INTO [MuevemeSchema].[User] (Name, LastName, Email, PhoneNumber, UserName, PasswordHash, PasswordSalt)
            VALUES ('{user.Name}', '{user.LastName}', '{user.Email}', '{user.PhoneNumber}', '{user.UserName}', @PasswordHash, @PasswordSalt)
        ";

        List<SqlParameter> sqlParameters = new List<SqlParameter>();

        SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
        passwordSaltParameter.Value = passwordSalt;

        SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
        passwordHashParameter.Value = passwordHash;

        sqlParameters.Add(passwordSaltParameter);
        sqlParameters.Add(passwordHashParameter);

        if(!_dapper.ExecuteSqlParameters(sqlAddUser, sqlParameters))
        {
            throw new Exception("Failed to add user");
        }

        return Ok();
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUser(int userId, UserUpdateDto user) 
    {   
        string sqlExistsUser = @$"
            SELECT * FROM [MuevemeSchema].[User]
            WHERE 
                (
                    Email = '{user.Email}' OR 
                    PhoneNumber = '{user.PhoneNumber}' OR 
                    UserName = '{user.UserName}' 
                ) AND 
                Id != {userId}
        ";

        IEnumerable<User> existsUser = _dapper.FindMany<User>(sqlExistsUser); 

        if(existsUser.Count() > 0) 
        {
            throw new Exception("Another account already exists with this email, phone number or user name");
        }

        string sqlUpdateUser = @"
            UPDATE [MuevemeSchema].[User]
            SET 
                Name = '" + user.Name  + @"', 
                LastName = '" + user.LastName  + @"', 
                Email = '" + user.Email  + @"', 
                PhoneNumber = '" + user.PhoneNumber  + @"', 
                UserName = '" + user.UserName  + @"', 
                PorfileImageUrl = '" + user.PorfileImageUrl  + @"'
        ";

        if(user.NewPassword.Length > 0) {
            byte[] passwordSalt = new byte[129/8];
            using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = _authHelper.GetPasswordHash(user.NewPassword, passwordSalt);

            sqlUpdateUser += @"
                , PasswordHash = @PasswordHash, 
                PasswordSalt = @PasswordSalt
            ";

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
            passwordSaltParameter.Value = passwordSalt;

            SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
            passwordHashParameter.Value = passwordHash;

            sqlParameters.Add(passwordSaltParameter);
            sqlParameters.Add(passwordHashParameter);

            sqlUpdateUser += " WHERE Id = " + userId.ToString();

            if(!_dapper.ExecuteSqlParameters(sqlUpdateUser, sqlParameters))
            {
                throw new Exception("Failed to update user");
            }

            return Ok();
        }

        sqlUpdateUser += " WHERE Id = " + userId.ToString();

        if(!_dapper.ExecuteSql(sqlUpdateUser)) 
        {
            throw new Exception("Failed to update user");
        }

        return Ok();
    }
}