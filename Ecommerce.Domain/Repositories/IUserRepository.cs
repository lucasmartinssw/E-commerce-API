using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories;

public interface IUserRepository
{
    // Contrato para adicionar um novo usuário
    Task Add(User user);

    // Contrato para verificar se um email já existe
    Task<bool> ExistsUserWithEmail(string email);

    // Contrato para obter um usuário pelo email
    Task<User>GetByEmail(string email);
}