
using FDP.Dtos.User;
using FDP.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FDP.Controller;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService=authService;
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser(RegisterUserDto dto)
    {
        var user=await _authService.RegisterUserAsync(dto);
        return Ok(user);

    }

    [HttpPost("login-user")]
    public async Task<IActionResult> LoginUser(LoginUserDto dto)
    {
        var loggedIn=await _authService.LoginUserAsnyc(dto);
        return Ok(loggedIn);

    }


}