using ECommerceApi.Application.DTOs;
using ECommerceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Interfaces.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(UserRegisterDto registerDto);
    Task<string> LoginAsync(UserLoginDto loginDto);
}
