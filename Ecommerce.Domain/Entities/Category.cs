using System.Collections.Generic;

namespace Ecommerce.Domain.Entities;

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

 
    public ICollection<Product> Products { get; set; } = new List<Product>();
}