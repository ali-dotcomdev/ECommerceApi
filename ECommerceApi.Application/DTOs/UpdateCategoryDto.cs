using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.DTOs;

public class UpdateCategoryDto
{
    public required string Name { get; set; }
    public Guid Id { get; set; }
}
