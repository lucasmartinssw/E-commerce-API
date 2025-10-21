using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories;

public interface IUserRepository
{
    Task Add(User user);

    Task<bool> ExistsUserWithEmail(string email);
    Task<User>GetByEmail(string email);
    Task<List<User>> GetAll();
    Task Update(User user);
}