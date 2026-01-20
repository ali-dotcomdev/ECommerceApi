using ECommerceApi.Application.Interfaces.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Infrastructure.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
                
    }
}
