using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
namespace Ecommerce.Infrastructure.DataAccess;
public class ProductRepository : IProductRepository
{
    private readonly EcommerceDbContext _context;
    public ProductRepository(EcommerceDbContext context) { _context = context; }

    public async Task Add(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
}