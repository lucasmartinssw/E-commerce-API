using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
namespace Ecommerce.Infrastructure.DataAccess;
public class OrderRepository : IOrderRepository
{
    private readonly EcommerceDbContext _context;
    public OrderRepository(EcommerceDbContext context) { _context = context; }

    public async Task Add(Order order)
    {
        await _context.Orders.AddAsync(order);
  
    }
}