using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Common;
namespace Ecommerce.Domain.Repositories;
public interface IProductRepository {
    Task Add(Product product);
    Task<PagedResult<Product>> GetAllPaged(
        int pageNumber,
        int pageSize,
        string? sortBy,
        long? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        string? searchTerm
    );
}