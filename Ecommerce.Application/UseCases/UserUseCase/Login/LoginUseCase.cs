using Ecommerce.Application.Security.Cryptography;
using Ecommerce.Application.Security.Token;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.UserUseCase.Login;

public class LoginUseCase
{
    private readonly IUserRepository _repository;
    private readonly JwtTokenGenerator _tokenGenerator;

    public LoginUseCase(IUserRepository repository, JwtTokenGenerator tokenGenerator)
    {
        _repository = repository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginUserJson request)
    {
        Validate(request);

        var user = await _repository.GetByEmail(request.Email);

        var passwordEncryptor = new PasswordEncryptor();
        bool passwordMatch = false;

        if (user != null)
        {
            passwordMatch = passwordEncryptor.Verify(request.Password, user.Password);
        }

       
        if (user == null || !passwordMatch)
        {
            throw new ValidationErrorsException(new List<string> { "Credenciais inválidas." });
        }



        var token = _tokenGenerator.GenerateToken(user.Email, user.Role.ToString());

        return new ResponseRegisteredUserJson
        {
            Token = token
        };
    }

    private void Validate(RequestLoginUserJson request)
    {
        var validator = new LoginValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorsException(errorMessages);
        }
    }
}