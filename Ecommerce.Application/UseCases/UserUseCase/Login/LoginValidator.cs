using Ecommerce.Communication.Requests;
using FluentValidation;

namespace Ecommerce.Application.UseCases.UserUseCase.Login;

public class LoginValidator : AbstractValidator<RequestLoginUserJson>
{
    public LoginValidator()
    {
        // Regra para o Email
        RuleFor(req => req.Email).NotEmpty().WithMessage("O e-mail não pode ser vazio.").EmailAddress().WithMessage("O e-mail informado é inválido.");

        // Regra para a Senha
        RuleFor(req => req.Password)
            .NotEmpty().WithMessage("A senha não pode ser vazia.");
    }
}