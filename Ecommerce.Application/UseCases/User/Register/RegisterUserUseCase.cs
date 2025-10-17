// Ecommerce.Application/UseCases/User/Register/RegisterUserUseCase.cs
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.UseCases.User.Register;

public class RegisterUserUseCase
{
    public ResponseRegisteredUserJson Execute(RequestRegisterUserJson request)
    {
        Validate(request);

        // TODO: Mapear o request para a entidade User
        // TODO: Criptografar a senha (hash)
        // TODO: Salvar o usuário no banco de dados
        // TODO: Gerar o token de autenticação (JWT)

        // Retorno temporário
        return new ResponseRegisteredUserJson
        {
            Token = "TOKEN_GERADO_AQUI"
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