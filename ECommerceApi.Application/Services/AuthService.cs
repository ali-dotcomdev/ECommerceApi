using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Interfaces.Repositories;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Application.Mappings;
using ECommerceApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceApi.Application.Services;

public class AuthService : IAuthService
{
    public readonly IGenericRepository<User> _userRepository;
    public readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration, ILogger<AuthService> logger, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
        _configuration = configuration;
        _logger = logger;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public RefreshToken GenerateSecureRandomToken()
    {
        Span<byte> secureBuffer = stackalloc byte[64];
        RandomNumberGenerator.Fill(secureBuffer);
        var buffer = Convert.ToBase64String(secureBuffer);

        var token = new RefreshToken
        {
            Token = buffer,
            Expires = DateTime.UtcNow.AddDays(1),
            Created = DateTime.UtcNow,

            CreatedByIp = "127.0.0.1"
        };
        return token;
    }

    public async Task<User> RegisterAsync(UserRegisterDto registerDto)
    {
        _logger.LogInformation("Yeni kayit istegi {Email}", registerDto.Email);
        var users = await _userRepository.GetAllAsync();
        if (users.Any(u => u.Email == registerDto.Email))
        {
            _logger.LogWarning("kayit basarisiz, email kullaniliyor {Email}", registerDto.Email);
            throw new Exception("Bu Email kullanimda");
        }
        var newUser = registerDto.ToUserEntity();

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, registerDto.Password);

        await _userRepository.AddAsync(newUser);

        _logger.LogInformation("Kullanici basarili bir sekilde olusturuldu {Email}", registerDto.Email);
        return newUser;
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto)
    {
        _logger.LogInformation("Giris denemesi yapiliyor: {Email}", loginDto.Email);
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == loginDto.Email);

        if (user == null)
        {
            _logger.LogWarning("Giris basarisiz kullanici bulunamadi: {Email}", loginDto.Email);
            throw new Exception("Kullanici veya sifre hatali: ");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Giris basarisiz sifre hatali: {Email}", loginDto.Email);
            throw new Exception("Kullanici bulunamadi veya sifre hatali.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] // payload
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["JwtSettings:Issuer"], //api
            Audience = _configuration["JwtSettings:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescription);

        _logger.LogInformation("Giris basarili. Token uretildi: {Email}", loginDto.Email);
        string accessTokenString = tokenHandler.WriteToken(token);

        var refreshTokenEntity = GenerateSecureRandomToken();
        refreshTokenEntity.UserId = user.Id;

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        return new AuthResponseDto
        {
            AccessToken = accessTokenString,
            RefreshToken = refreshTokenEntity.Token,
            RefreshTokenExpiration = refreshTokenEntity.Expires
        };
    }
}
