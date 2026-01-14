using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Application.Mappings;
using ECommerceApi.Application.Wrapper;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCategoryDto categoryDto)
    {
        var result = await _categoryService.CreateCategoryAsync(categoryDto);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilter pagination)
    {
        var result = await _categoryService.GetAllCategoryAsync(pagination);
        return Ok(result);
    }
}
