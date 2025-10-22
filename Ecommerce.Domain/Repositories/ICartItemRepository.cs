using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories;
public interface ICartItemRepository
{
    Task<CartItem?> GetByCartAndProduct(long cartId, long productId);
    Task Add(CartItem item);
    Task Update(CartItem item);
    Task<CartItem?> GetById(long id);
    Task Delete(CartItem item);
}