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
    public Task<PagedResponse<List<CategoryResponseDto>>> GetAllCategoriesAsync(PaginationFilter filter);
    public Task DeleteCategoryAsync(Guid id);
    public Task UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategory);
}
