using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Infrastructure.DataAccess;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Addresses.GetAll;

public class GetAllAddressesUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;

    public GetAllAddressesUseCase(IUserRepository userRepository, IAddressRepository addressRepository)
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }

    public async Task<ResponseAllAddressesJson> Execute(string userEmail)
    {
       
        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não encontrado." });
        }

       
        var addresses = await _addressRepository.GetAllByUserId(user.Id);

        return new ResponseAllAddressesJson
        {
            Addresses = addresses.Select(addr => new ResponseAddressJson
            {
                Id = addr.Id,
                Street = addr.Street,
                Number = addr.Number,
                Complement = addr.Complement,
                Neighborhood = addr.Neighborhood,
                City = addr.City,
                State = addr.State,
                ZipCode = addr.ZipCode
            }).ToList()
        };
    }
}