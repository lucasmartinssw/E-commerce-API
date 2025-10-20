using Ecommerce.Communication.Requests;
using FluentValidation;
namespace Ecommerce.Application.UseCases.Products.Create;

public class CreateProductValidator : AbstractValidator<RequestCreateProductJson>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Nome do produto é obrigatório.");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Preço deve ser maior que zero.");
        RuleFor(p => p.StockQuantity).GreaterThanOrEqualTo(0).WithMessage("Estoque não pode ser negativo.");
        RuleFor(p => p.CategoryId).GreaterThan(0).WithMessage("ID da Categoria é obrigatório.");
    }
}