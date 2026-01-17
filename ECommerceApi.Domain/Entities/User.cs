using ECommerceApi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "Customer";

    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
