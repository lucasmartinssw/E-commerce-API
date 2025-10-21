using Ecommerce.Application.Services.ViaCep;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Addresses.AddByCep;

public class AddAddressByCepUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly ViaCepService _viaCepService;

    public AddAddressByCepUseCase(
        IUserRepository userRepository,
        IAddressRepository addressRepository,
        ViaCepService viaCepService)
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _viaCepService = viaCepService;
    }

    public async Task<ResponseAddressJson> Execute(string userEmail, RequestAddAddressByCepJson request)
    {

        var viaCepResponse = await _viaCepService.GetAddressByCepAsync(request.ZipCode);
        if (viaCepResponse == null)
        {
            throw new ValidationErrorsException(new List<string> { "CEP não encontrado ou inválido." });
        }

        
        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não encontrado." });
        }

       
        var address = new Address
        {
            
            Street = viaCepResponse.Street,
            Neighborhood = viaCepResponse.Neighborhood,
            City = viaCepResponse.City,
            State = viaCepResponse.State.ToUpper(),
            ZipCode = viaCepResponse.Cep.Replace("-", ""),
            Number = request.Number,
            Complement = request.Complement,
            UserId = user.Id
        };

        await _addressRepository.Add(address);

       
        return new ResponseAddressJson
        {
            Id = address.Id,
            Street = address.Street,
            Number = address.Number,
            Complement = address.Complement,
            Neighborhood = address.Neighborhood,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode
        };
    }
}