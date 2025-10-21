using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Infrastructure.DataAccess;

public class CartItemRepository : ICartItemRepository
{
    private readonly EcommerceDbContext _context;
    public CartItemRepository(EcommerceDbContext context) { _context = context; }

    public async Task<CartItem?> GetByCartAndProduct(long cartId, long productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }
    public async Task Add(CartItem item)
    {
        await _context.CartItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }
    public async Task Update(CartItem item)
    {
        _context.CartItems.Update(item);
        await _context.SaveChangesAsync();
    }
}