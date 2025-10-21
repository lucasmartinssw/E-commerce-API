using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Addresses.Delete;

public class DeleteAddressUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;

    public DeleteAddressUseCase(IUserRepository userRepository, IAddressRepository addressRepository)
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }

    public async Task Execute(string userEmail, long addressId)
    {
        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não autenticado." });
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

        await _addressRepository.Delete(address);
    }
}