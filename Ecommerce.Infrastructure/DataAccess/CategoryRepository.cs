using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Infrastructure.DataAccess;
using Ecommerce.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
public class CategoryRepository : ICategoryRepository
{
    private readonly EcommerceDbContext _context;
    public CategoryRepository(EcommerceDbContext context) { _context = context; }

    public async Task<bool> Exists(long categoryId)
    {
        return await _context.Categories.AnyAsync(c => c.Id == categoryId);
    }

    public async Task Add(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByName(string name)
    {
        return await _context.Categories.AnyAsync(c => c.Name.Equals(name));
    }

    public async Task<bool> ExistsBySlug(string slug)
    {
        return await _context.Categories.AnyAsync(c => c.Slug.Equals(slug));
    }

    public async Task<List<Category>> GetAll()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category?> GetById(long id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> IsInUse(long id)
    {
        return await _context.Products.AnyAsync(p => p.CategoryId == id);
    }

    public async Task Delete(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}