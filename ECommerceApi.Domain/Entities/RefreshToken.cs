using ECommerceApi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }

    public bool IsExpire => DateTime.UtcNow >= Expires;
    public bool IsActive => Revoked == null && !IsExpire;

    public Guid UserId { get; set; }
    public User User { get; set; }
}
