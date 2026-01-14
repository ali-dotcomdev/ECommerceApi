using ECommerceApi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Domain.Entities;

public class Category : BaseEntity
{
    public Category()
    {
        // Kategori olustugunda bos bir urun sepeti olussun, zorunluluk gibi bir sey.
        // yoksa NullReference hatasi alirsin.
        Products = new HashSet<Product>();
    }
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}
