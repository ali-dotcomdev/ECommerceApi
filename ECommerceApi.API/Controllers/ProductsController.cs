using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Application.Mappings;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[Route("api/[controller]")] 
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateProductDto productDto)
    {
        await _productService.CreateProductAsync(productDto);
        return Ok("Product created successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductDto updateDto)
    {
        await _productService.UpdateProductAsync(id, updateDto);
        return Ok("Product updated successfully");
    }

    [Authorize]
    [HttpGet("secret-products")]
    public IActionResult GetSecretProducts()
    {
        return Ok("JWT");
    }
}
