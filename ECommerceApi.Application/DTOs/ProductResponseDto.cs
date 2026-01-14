using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.DTOs;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
}
