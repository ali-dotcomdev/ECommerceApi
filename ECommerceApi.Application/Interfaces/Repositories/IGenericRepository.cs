using ECommerceApi.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace ECommerceApi.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : BaseEntity 
{
    public Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    public Task<T> GetByIdAsync(Guid id);
    public Task AddAsync(T entity);
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(T entity);
}
