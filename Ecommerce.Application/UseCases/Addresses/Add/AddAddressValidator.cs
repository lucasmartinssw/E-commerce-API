using Ecommerce.Communication.Requests;
using FluentValidation;

namespace Ecommerce.Application.UseCases.Addresses.Add;

public class AddAddressValidator : AbstractValidator<RequestAddAddressJson>
{
    public AddAddressValidator()
    {
        RuleFor(x => x.Street).NotEmpty().WithMessage("O nome da rua é obrigatório.");
        RuleFor(x => x.Neighborhood).NotEmpty().WithMessage("O bairro é obrigatório.");
        RuleFor(x => x.City).NotEmpty().WithMessage("A cidade é obrigatória.");

        RuleFor(x => x.State).NotEmpty()
            .Length(2).WithMessage("O estado deve ter exatamente 2 caracteres (ex: 'MG').");

        RuleFor(x => x.ZipCode).NotEmpty()
            .Matches(@"^\d{8}$").WithMessage("O CEP deve conter 8 dígitos (apenas números).");
    }
}