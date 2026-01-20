using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.DTOs;

public class RefreshTokenRequestDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
