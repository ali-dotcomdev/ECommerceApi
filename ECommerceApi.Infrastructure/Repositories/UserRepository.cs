using ECommerceApi.Application.Interfaces.Repositories;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Infrastructure.Data;
using ECommerceApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ICacheService _cacheService;
    public UserRepository(AppDbContext options, ICacheService cacheService) : base(options) 
    {
        _cacheService = cacheService;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        string cacheKey = $"User_{email}";

        var cachedUser = await _cacheService.GetAsync<User>(cacheKey);
        if (cachedUser != null)
        {
            return cachedUser;
        }

        var dbUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (dbUser != null)
        {
            await _cacheService.SetAsync<User>(cacheKey, dbUser, TimeSpan.FromMinutes(30));
        }

        return dbUser;
    }
}
