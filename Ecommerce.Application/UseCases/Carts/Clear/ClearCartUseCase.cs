using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Carts.Clear;

public class ClearCartUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;

    public ClearCartUseCase(IUserRepository userRepository, ICartRepository cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task Execute(string userEmail)
    {

        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não autenticado." });
        }


        var cart = await _cartRepository.GetByUserId(user.Id); 
        if (cart != null)
        {
            await _cartRepository.ClearItems(cart.Id);
        }
    }
}