using Microsoft.AspNetCore.Mvc;
using MuevemeApi.Data;
using MuevemeApi.Models;

namespace MuevemeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IRepository<User> _userRepository;

    public UserController(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userRepository.GetAllAsync();
    }
}