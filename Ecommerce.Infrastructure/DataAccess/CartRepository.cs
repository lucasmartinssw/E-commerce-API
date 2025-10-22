using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Infrastructure.DataAccess;

public class CartRepository : ICartRepository
{
    private readonly EcommerceDbContext _context;
    public CartRepository(EcommerceDbContext context) { _context = context; }

    public async Task<Cart?> GetByUserId(long userId)
    {
        return await _context.Carts
            .Include(c => c.Items) 
                .ThenInclude(item => item.Product) 
                    .ThenInclude(p => p.Category) 
            .AsNoTracking() 
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
    public async Task Add(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }
    public async Task ClearItems(long cartId)
    {
       
        await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .ExecuteDeleteAsync();
    }

}