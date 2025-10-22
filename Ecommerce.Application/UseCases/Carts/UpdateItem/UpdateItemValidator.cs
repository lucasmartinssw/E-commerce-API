using Ecommerce.Communication.Requests;
using FluentValidation;

namespace Ecommerce.Application.UseCases.Carts.UpdateItem;

public class UpdateItemValidator : AbstractValidator<RequestUpdateCartItemJson>
{
    public UpdateItemValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("A quantidade deve ser de pelo menos 1.");
    }
}