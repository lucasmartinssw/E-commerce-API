using Ecommerce.Application.Security.Cryptography; 
using Ecommerce.Communication.Requests;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.UserUseCase.ChangePassword;

public class ChangePasswordUseCase
{
    private readonly IUserRepository _repository;
    private readonly PasswordEncryptor _passwordEncryptor;

    public ChangePasswordUseCase(IUserRepository repository)
    {
        _repository = repository;
        _passwordEncryptor = new PasswordEncryptor(); 
    }

    public async Task Execute(string userEmail, RequestChangePasswordJson request)
    {
        Validate(request);

        var user = await _repository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não encontrado." });
        }
        var passwordMatch = _passwordEncryptor.Verify(request.CurrentPassword, user.Password);
        if (!passwordMatch)
        {
            throw new ValidationErrorsException(new List<string> { "A senha atual está incorreta." });
        }

        var newHashedPassword = _passwordEncryptor.Encrypt(request.NewPassword);

        user.Password = newHashedPassword;
        user.UpdatedAt = DateTime.UtcNow;

        await _repository.Update(user);
    }

    private void Validate(RequestChangePasswordJson request)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationErrorsException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}