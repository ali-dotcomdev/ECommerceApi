using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.DTOs;

public class CreateProductDto
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }
}
