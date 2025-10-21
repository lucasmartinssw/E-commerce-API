using Ecommerce.Communication.Requests;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.UserUseCase.UpdateProfile;

public class UpdateUserProfileUseCase
{
    private readonly IUserRepository _repository;

    public UpdateUserProfileUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task Execute(string userEmail, RequestUpdateUserProfileJson request)
    {
        Validate(request);

        var user = await _repository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não encontrado." });
        }

        user.Name = request.Name;
        user.Telephone = request.Telephone;
        user.UpdatedAt = DateTime.UtcNow; 

        
        await _repository.Update(user);
    }

    private void Validate(RequestUpdateUserProfileJson request)
    {
        var validator = new UpdateUserProfileValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationErrorsException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}