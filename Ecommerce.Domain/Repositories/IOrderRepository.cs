using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IOrderRepository { 
    Task Add(Order order);
    Task<List<Order>> GetAllByUserId(long userId);
} 