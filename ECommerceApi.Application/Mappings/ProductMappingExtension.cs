using ECommerceApi.Application.DTOs;
using ECommerceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Mappings;

public static class ProductMappingExtension
{
    public static Product ToProductEntity(this CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };
    }

    public static void UpdateProduct(this Product product, UpdateProductDto productDto)
    {
        product.Name = productDto.Name;
        product.Price = productDto.Price;
        product.Stock = productDto.Stock;
    }

    public static ProductResponseDto ToProductResponseEntity(this Product product)
    {
        return new ProductResponseDto 
        {
            CategoryId = product.Id,
            CategoryName = product.Category?.Name,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}
