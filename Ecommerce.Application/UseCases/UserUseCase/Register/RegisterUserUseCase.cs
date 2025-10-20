// Ecommerce.Application/UseCases/User/Register/RegisterUserUseCase.cs
using Ecommerce.Application.Security.Cryptography;
using Ecommerce.Application.Security.Token;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.UserUseCase.Register;

public class RegisterUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly JwtTokenGenerator _tokenGenerator;
    public RegisterUserUseCase(IUserRepository repository, JwtTokenGenerator tokenGenerator)
    {
        _repository = repository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
       
        Validate(request);

     
        var userWithSameEmail = await _repository.ExistsUserWithEmail(request.Email);
        if (userWithSameEmail)
        {
          
            throw new ValidationErrorsException(new List<string> { "Este e-mail já está em uso." });
        }

        
        var passwordEncryptor = new PasswordEncryptor();
        var hashedPassword = passwordEncryptor.Encrypt(request.Password);

        
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Telephone = request.Telephone,
            Password = hashedPassword
        };

       
        await _repository.Add(user);

        var token = _tokenGenerator.GenerateToken(user.Email, user.Role.ToString());

        return new ResponseRegisteredUserJson
        {
            Token = token
        };
    }

    private void Validate(RequestRegisterUserJson request)
    {
        var validator = new UserValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorsException(errorMessages);
        }
    }
}