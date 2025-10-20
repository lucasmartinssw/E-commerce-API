using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Infrastructure.DataAccess;
public class CategoryRepository : ICategoryRepository
{
    private readonly EcommerceDbContext _context;
    public CategoryRepository(EcommerceDbContext context) { _context = context; }

    public async Task<bool> Exists(long categoryId)
    {
        return await _context.Categories.AnyAsync(c => c.Id == categoryId);
    }
}