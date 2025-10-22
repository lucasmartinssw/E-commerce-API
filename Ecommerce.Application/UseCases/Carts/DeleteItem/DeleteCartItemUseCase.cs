using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Carts.DeleteItem;

public class DeleteCartItemUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ICartRepository _cartRepository; 

    public DeleteCartItemUseCase(
        IUserRepository userRepository,
        ICartItemRepository cartItemRepository,
        ICartRepository cartRepository) 
    {
        _userRepository = userRepository;
        _cartItemRepository = cartItemRepository;
        _cartRepository = cartRepository; 
    }

    public async Task Execute(string userEmail, long itemId)
    {
        
        var user = await _userRepository.GetByEmail(userEmail);
        
        var cart = user != null ? await _cartRepository.GetByUserId(user.Id) : null;

        if (user == null || cart == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário ou carrinho não encontrado." });
        }

        var cartItem = await _cartItemRepository.GetById(itemId);
        if (cartItem == null)
        {
            throw new ValidationErrorsException(new List<string> { "Item do carrinho não encontrado." });
        }

        if (cartItem.CartId != cart.Id)
        {
            throw new ValidationErrorsException(new List<string> { "Acesso negado ao item do carrinho." });
        }

        await _cartItemRepository.Delete(cartItem);
    }
}