using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Interfaces.Repositories;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerceApi.Application.Wrapper;

namespace ECommerceApi.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _repository;
    public CategoryService(IGenericRepository<Category> repository)
    {
        _repository = repository;
    }
    public async Task<CategoryResponseDto> GetByIdAsync(Guid id)
    {
        var currentCategory = await _repository.GetByIdAsync(id);
        if (currentCategory == null) throw new Exception("Aradiginiz kategoride urun yoktur.");
        return currentCategory.ToCategoryResponseEntity();
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto categoryDto)
    {
        var category = categoryDto.ToCategoryEntity();
        await _repository.AddAsync(category);
        return category.ToCategoryResponseEntity();
    }

    public async Task<PagedResponse<List<CategoryResponseDto>>> GetAllCategoryAsync(PaginationFilter filter)
    {
        var allCategories = await _repository.GetAllAsync();

        var categoryDtos = allCategories
                           .Select(c => c.ToCategoryResponseEntity())
                           .ToList();

        var pagedData = categoryDtos
                        .Skip((filter.PageNumber - 1) * filter.PageSize)
                        .Take(filter.PageSize)
                        .ToList();

        var totalRecords = categoryDtos.Count;

        return new PagedResponse<List<CategoryResponseDto>>(pagedData, filter.PageNumber, filter.PageSize, totalRecords);
    }
}
