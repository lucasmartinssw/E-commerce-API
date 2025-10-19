// Ecommerce.Application/UseCases/User/Register/UserValidator.cs
using Ecommerce.Communication.Requests;
using FluentValidation;

namespace Ecommerce.Application.UseCases.UserUseCase.Register;

public class UserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public UserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("O nome não pode ser vazio.");
        RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("E-mail inválido.");
        RuleFor(user => user.Telephone).NotEmpty().WithMessage("O telefone é obrigatório.");
        RuleFor(user => user.Password).MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
    }
}   