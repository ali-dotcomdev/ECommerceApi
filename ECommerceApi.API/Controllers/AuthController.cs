using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
    {
        var user = await _authService.RegisterAsync(registerDto);
        return Ok("Basarili bir sekilde kayit yapildi");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);
        return Ok(new {Token = token});
    }
}
