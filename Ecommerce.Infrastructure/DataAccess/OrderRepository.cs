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
}