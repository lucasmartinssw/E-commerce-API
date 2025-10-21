using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
namespace Ecommerce.Infrastructure.DataAccess;
using Ecommerce.Domain.Common;
using Microsoft.EntityFrameworkCore;
public class ProductRepository : IProductRepository
{
    private readonly EcommerceDbContext _context;
    public ProductRepository(EcommerceDbContext context) { _context = context; }

    public async Task Add(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Product>> GetAllPaged(
        int pageNumber, int pageSize, string? sortBy,
        long? categoryId, decimal? minPrice, decimal? maxPrice, string? searchTerm)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsNoTracking();

      
        if (categoryId.HasValue && categoryId > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.Name.ToLower().Contains(searchTerm.ToLower()) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTerm.ToLower()))
            );
        }

        switch (sortBy?.ToLower())
        {
            case "price-asc":
                query = query.OrderBy(p => p.Price);
                break;
            case "price-desc":
                query = query.OrderByDescending(p => p.Price);
                break;
            case "name-asc":
                query = query.OrderBy(p => p.Name);
                break;
            case "name-desc":
                query = query.OrderByDescending(p => p.Name);
                break;
            default:
                query = query.OrderBy(p => p.Name); 
                break;
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
    public async Task<Product?> GetById(long id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

}