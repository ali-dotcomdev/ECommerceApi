using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Interfaces.Repositories;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Application.Mappings;
using ECommerceApi.Application.Settings;
using ECommerceApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<AuthService> _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;

    public AuthService(ILogger<AuthService> logger, IRefreshTokenRepository refreshTokenRepository, IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
    {
        _passwordHasher = new PasswordHasher<User>();
        _logger = logger;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }

    private string GenerateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] // payload
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer, //api
            Audience = _jwtSettings.Audience
        };
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public RefreshToken GenerateSecureRandomToken()
    {
        Span<byte> secureBuffer = stackalloc byte[64];
        RandomNumberGenerator.Fill(secureBuffer);
        var buffer = Convert.ToBase64String(secureBuffer);

        var token = new RefreshToken
        {
            Token = buffer,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Created = DateTime.UtcNow,

            CreatedByIp = "127.0.0.1"
        };
        return token;
    }

    public async Task<User> RegisterAsync(UserRegisterDto registerDto)
    {
        _logger.LogInformation("Yeni kayit istegi {Email}", registerDto.Email);

        var user = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (user != null)
        {
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
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null)
        {
            _logger.LogWarning("Giris basarisiz kullanici bulunamadi: {Email}", loginDto.Email);
            throw new KeyNotFoundException("kullanici bulunamadi");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Giris basarisiz sifre hatali: {Email}", loginDto.Email);
            throw new ArgumentException("sifre hatali");
        }

        var refreshTokenEntity = GenerateSecureRandomToken();
        refreshTokenEntity.UserId = user.Id;

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        return new AuthResponseDto
        {
            AccessToken = GenerateJwt(user),
            RefreshToken = refreshTokenEntity.Token,
            RefreshTokenExpiration = refreshTokenEntity.Expires
        };
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_jwtSettings.SecretKey)  
            ),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(
            token,
            validationParameters,
            out SecurityToken securityToken
        );

        if (securityToken is not JwtSecurityToken jwtToken || 
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid Token");
        }

        return principal;
    }

    public async Task<AuthResponseDto> RefreshTokenLoginAsync(RefreshTokenRequestDto request)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new SecurityTokenException("Invalid access token or refresh token");

        var userIdString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out Guid userId)) throw new SecurityTokenException("Invalid user id in token");

        var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (storedRefreshToken == null) throw new Exception("Invalid refresh token");

        if (storedRefreshToken.Expires < DateTime.UtcNow) throw new SecurityTokenException("Refresh Token expired. Please login again.");
        if (storedRefreshToken.Revoked != null) throw new SecurityTokenException("This token has been revoked.");
        if (storedRefreshToken.UserId != userId) throw new SecurityTokenException("Invalid token owner");

        storedRefreshToken.Revoked = DateTime.UtcNow;

        await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

        var user = await _userRepository.GetByIdAsync(userId);

        var newAccessToken = GenerateJwt(user);

        var newRefreshTokenEntity = GenerateSecureRandomToken();
        newRefreshTokenEntity.UserId = user.Id;

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);

        return new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenEntity.Token,
            RefreshTokenExpiration = newRefreshTokenEntity.Expires
        };
    }
}
