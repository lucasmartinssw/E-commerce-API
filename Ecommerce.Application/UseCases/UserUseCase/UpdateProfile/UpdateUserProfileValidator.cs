using Ecommerce.Communication.Requests;
using FluentValidation;

namespace Ecommerce.Application.UseCases.UserUseCase.UpdateProfile;

public class UpdateUserProfileValidator : AbstractValidator<RequestUpdateUserProfileJson>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome não pode ser vazio.");

        RuleFor(x => x.Telephone)
            .NotEmpty().WithMessage("O telefone não pode ser vazio.");
    }
}