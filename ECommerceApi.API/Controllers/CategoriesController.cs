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

public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCategoryDto categoryDto)
    {
        var result = await _categoryService.CreateCategoryAsync(categoryDto);
       
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilter pagination)
    {
        var result = await _categoryService.GetAllCategoriesAsync(pagination);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCategoryDto updateCategory)
    {
        if (id != updateCategory.Id) return BadRequest("ID mismatch");
        await _categoryService.UpdateCategoryAsync(id, updateCategory);
        return Ok("Category updated successfully.");
    }
}
