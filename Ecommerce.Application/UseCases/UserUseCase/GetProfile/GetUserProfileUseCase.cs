using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.UserUseCase.GetProfile;

public class GetUserProfileUseCase
{
    private readonly IUserRepository _repository;

    public GetUserProfileUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseUserJson> Execute(string userEmail)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não autenticado." });
        }

        var user = await _repository.GetByEmail(userEmail);

        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não encontrado." });
        }

        return new ResponseUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }
}