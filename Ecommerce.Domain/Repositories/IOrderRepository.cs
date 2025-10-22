using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories;
public interface IOrderRepository { Task Add(Order order); } 