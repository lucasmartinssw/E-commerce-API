namespace Ecommerce.Domain.Repositories;
using Ecommerce.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
public interface ICategoryRepository { 
    Task<bool> Exists(long categoryId);
    Task Add(Category category);
    Task<bool> ExistsByName(string name);
    Task<bool> ExistsBySlug(string slug);

    Task<List<Category>> GetAll();
    Task<Category?> GetById(long id);
    Task<bool> IsInUse(long id);
    Task Delete(Category category);
}