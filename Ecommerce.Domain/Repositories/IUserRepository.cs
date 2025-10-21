using Ecommerce.Domain.Entities;
using System.Collections.Generic;
namespace Ecommerce.Domain.Repositories;

public interface IUserRepository
{
    Task Add(User user);

    Task<bool> ExistsUserWithEmail(string email);
    Task<User>GetByEmail(string email);
    Task<List<User>> GetAll();
    Task Update(User user);
    
}