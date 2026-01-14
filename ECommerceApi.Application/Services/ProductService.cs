using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Interfaces.Repositories;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Application.Mappings;
using ECommerceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace ECommerceApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _repository;

    public ProductService(IGenericRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task CreateProductAsync(CreateProductDto productDto)
    {
        if (productDto.Price < 0) throw new Exception("Fiyat negatif olamaz");
        var product = productDto.ToProductEntity();

        await _repository.AddAsync(product);
    }
    
    public async Task DeleteProductAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) throw new Exception("Silinecek urun bulunamadi");

        await _repository.DeleteAsync(id);
    }

    public async Task<List<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _repository.GetAllAsync(p => p.Category);

        return products.Select(p => p.ToProductResponseEntity()).ToList();
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductDto productDto)
    {
        var currentProduct = await _repository.GetByIdAsync(id);
        if (productDto.Price < 0) throw new Exception("Fiyat negatif olamaz");
        if (currentProduct == null) throw new Exception("Aradiginiz kriterlerde urun bulunamamistir");
        currentProduct.UpdateProduct(productDto);

        await _repository.UpdateAsync(currentProduct);
    }
}
