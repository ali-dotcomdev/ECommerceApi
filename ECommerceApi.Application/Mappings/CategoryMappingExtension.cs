using ECommerceApi.Application.DTOs;
using ECommerceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Mappings;

public static class CategoryMappingExtension
{
    public static Category ToCategoryEntity(this CreateCategoryDto createCategoryDto)
    {
        return new Category 
        {
            Name = createCategoryDto.Name
        };
    }

    public static CategoryResponseDto ToCategoryResponseEntity(this Category category)
    {
        return new CategoryResponseDto
        {
            Name = category.Name,
            Id = category.Id
        };
    }
}
