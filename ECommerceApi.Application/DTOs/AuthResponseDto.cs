using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.DTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
