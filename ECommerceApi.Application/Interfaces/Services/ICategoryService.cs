using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Wrapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Interfaces.Services;

public interface ICategoryService
{
    public Task<CategoryResponseDto> GetByIdAsync(Guid id);
    public Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto categoryDto);
    public Task<PagedResponse<List<CategoryResponseDto>>> GetAllCategoryAsync(PaginationFilter filter);
}
