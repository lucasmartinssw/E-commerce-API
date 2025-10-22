using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories;
public interface ICartRepository
{
    Task<Cart?> GetByUserId(long userId);
    Task Add(Cart cart);
    Task ClearItems(long cartId);
}