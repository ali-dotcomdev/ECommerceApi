using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Application.Wrapper;
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
        return Ok(Result<Guid>.Success(user.Id, "basariyla kayit olundu"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);
        return Ok(Result<AuthResponseDto>.Success(token, "giris basarili"));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var authResponse = await _authService.RefreshTokenLoginAsync(request);
        return Ok(Result<AuthResponseDto>.Success(authResponse, "token basarili bir sekilde yenilendi"));
    }
}
