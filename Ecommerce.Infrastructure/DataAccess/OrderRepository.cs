using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Infrastructure.DataAccess;
public class OrderRepository : IOrderRepository
{
    private readonly EcommerceDbContext _context;
    public OrderRepository(EcommerceDbContext context) { _context = context; }

    public async Task Add(Order order)
    {
        await _context.Orders.AddAsync(order);
  
    }
    public async Task<List<Order>> GetAllByUserId(long userId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }
    public async Task<List<Order>> GetAllAdmin()
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.User) 
            .OrderByDescending(o => o.OrderDate) 
            .ToListAsync();
    }

    public async Task<Order?> GetByIdTracked(long id)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task Update(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}