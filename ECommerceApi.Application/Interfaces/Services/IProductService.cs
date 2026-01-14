using ECommerceApi.Application.DTOs;
using ECommerceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Interfaces.Services;

public interface IProductService
{
    public Task<List<ProductResponseDto>> GetAllProductsAsync();
    public Task<ProductResponseDto> GetProductByIdAsync(Guid id);
    public Task<ProductResponseDto> CreateProductAsync(CreateProductDto productDto);
    public Task DeleteProductAsync(Guid id);
    public Task UpdateProductAsync(Guid id, UpdateProductDto productDto);
}
