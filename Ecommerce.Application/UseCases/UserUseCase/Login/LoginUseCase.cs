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

    public LoginUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginUserJson request)
    {
        // 1. [MUDANÇA] Validar a requisição usando o FluentValidation
        Validate(request);

        // 2. Buscar o usuário no banco
        var user = await _repository.GetByEmail(request.Email);

        // 3. Verificar a senha
        var passwordEncryptor = new PasswordEncryptor();
        bool passwordMatch = false;

        if (user != null)
        {
            passwordMatch = passwordEncryptor.Verify(request.Password, user.Password);
        }

        // 4. Se o usuário não existir OU a senha não bater, retorne o mesmo erro
        if (user == null || !passwordMatch)
        {
            throw new ValidationErrorsException(new List<string> { "Credenciais inválidas." });
        }

        // 5. Se deu tudo certo, gere o token
        var tokenGenerator = new JwtTokenGenerator();
        var token = tokenGenerator.GenerateToken(user.Email);

        return new ResponseRegisteredUserJson
        {
            Token = token
        };
    }

    // 6. [NOVO MÉTODO] Adicione este método privado à classe
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