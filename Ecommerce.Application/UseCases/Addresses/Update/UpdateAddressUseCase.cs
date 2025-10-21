using Ecommerce.Application.UseCases.Addresses.Add;
using Ecommerce.Communication.Requests;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Addresses.Update;

public class UpdateAddressUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;

    public UpdateAddressUseCase(IUserRepository userRepository, IAddressRepository addressRepository)
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }

    public async Task Execute(string userEmail, long addressId, RequestAddAddressJson request)
    {
    
        Validate(request);

     
        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não encontrado." });
        }

        
        var address = await _addressRepository.GetById(addressId);
        if (address == null)
        {
            throw new ValidationErrorsException(new List<string> { "Endereço não encontrado." });
        }

       
        if (address.UserId != user.Id)
        {
            
            throw new ValidationErrorsException(new List<string> { "Acesso negado." });
        }

       
        address.Street = request.Street;
        address.Number = request.Number;
        address.Complement = request.Complement;
        address.Neighborhood = request.Neighborhood;
        address.City = request.City;
        address.State = request.State.ToUpper();
        address.ZipCode = request.ZipCode;

        
        await _addressRepository.Update(address);
    }

    
    private void Validate(RequestAddAddressJson request)
    {
        var validator = new AddAddressValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationErrorsException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}