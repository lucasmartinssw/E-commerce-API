using Ecommerce.Communication.Requests;
using FluentValidation;
namespace Ecommerce.Application.UseCases.Carts;

public class AddItemValidator : AbstractValidator<RequestAddItemToCartJson>
{
    public AddItemValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("ID do produto inválido.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("A quantidade deve ser de pelo menos 1.");
    }
}