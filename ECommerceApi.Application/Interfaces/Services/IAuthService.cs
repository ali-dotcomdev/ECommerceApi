using ECommerceApi.Application.DTOs;
using ECommerceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Interfaces.Services;

public interface IAuthService
{
    public RefreshToken GenerateSecureRandomToken();
    Task<User> RegisterAsync(UserRegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
}
