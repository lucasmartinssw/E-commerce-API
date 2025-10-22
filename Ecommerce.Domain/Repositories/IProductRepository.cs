using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities;
using System.Threading.Tasks;
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
    Task<Product?> GetById(long id);
    Task UpdateRange(List<Product> products);
}