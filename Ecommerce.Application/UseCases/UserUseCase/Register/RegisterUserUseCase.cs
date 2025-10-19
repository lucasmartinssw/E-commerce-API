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

    public RegisterUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        // 4. Validação (continua igual)
        Validate(request);

        // 5. [NOVO] Verifica se o e-mail já existe no banco
        var userWithSameEmail = await _repository.ExistsUserWithEmail(request.Email);
        if (userWithSameEmail)
        {
            // Lança um erro de validação específico
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

        // 8. [TODO CORRIGIDO] Salvar o usuário no banco
        await _repository.Add(user);

        // 9. [TODO CORRIGIDO] Gerar o token de autenticação
        var tokenGenerator = new JwtTokenGenerator();
        var token = tokenGenerator.GenerateToken(user.Email);

        // 10. Retorna a resposta final com o token
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