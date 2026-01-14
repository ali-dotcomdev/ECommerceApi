using ECommerceApi.Application.DTOs;
using ECommerceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Mappings;

public static class UserMappingExtension
{
    public static User ToUserEntity(this UserRegisterDto dto)
    {
        return new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Role = "Customer",
            CreatedDate = DateTime.UtcNow
        };
    }
}
