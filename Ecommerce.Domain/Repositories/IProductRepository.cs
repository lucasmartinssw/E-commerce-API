using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories;
public interface IProductRepository { Task Add(Product product); }