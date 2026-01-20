using ECommerceApi.Application.DTOs;
using ECommerceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ECommerceApi.Application.Interfaces.Services;

public interface IAuthService
{
    public RefreshToken GenerateSecureRandomToken();
    Task<User> RegisterAsync(UserRegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
    public Task<AuthResponseDto> RefreshTokenLoginAsync(RefreshTokenRequestDto request);
}
