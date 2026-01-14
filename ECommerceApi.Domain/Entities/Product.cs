using ECommerceApi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Domain.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
 }
