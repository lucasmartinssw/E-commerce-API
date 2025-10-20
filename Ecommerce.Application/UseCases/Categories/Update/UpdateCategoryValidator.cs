using Ecommerce.Communication.Requests;
using FluentValidation;

namespace Ecommerce.Application.UseCases.Categories.Update;

public class UpdateCategoryValidator : AbstractValidator<RequestUpdateCategoryJson>
{
    public UpdateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Nome da categoria é obrigatório.")
            .MinimumLength(3).WithMessage("Nome da categoria deve ter no mínimo 3 caracteres.");
    }
}