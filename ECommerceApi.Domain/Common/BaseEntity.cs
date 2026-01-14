using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } // soft delete??
}
