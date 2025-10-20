namespace Ecommerce.Domain.Repositories;
public interface ICategoryRepository { Task<bool> Exists(long categoryId); }