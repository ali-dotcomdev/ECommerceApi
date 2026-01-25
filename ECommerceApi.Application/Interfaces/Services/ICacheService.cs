using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Interfaces.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null); // null - default
    Task RemoveAsync(string key);
}
