using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Carts.UpdateItem;

public class UpdateCartItemUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository; 

    public UpdateCartItemUseCase(
        IUserRepository userRepository,
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository)
    {
        _userRepository = userRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
    }

    
    public async Task<ResponseCartItemJson> Execute(string userEmail, long itemId, RequestUpdateCartItemJson request)
    {
       
        Validate(request);

        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null || user.Cart == null) 
        {
            throw new ValidationErrorsException(new List<string> { "Usuário ou carrinho não encontrado." });
        }

        
        var cartItem = await _cartItemRepository.GetById(itemId);
        if (cartItem == null)
        {
            throw new ValidationErrorsException(new List<string> { "Item do carrinho não encontrado." });
        }

        
        if (cartItem.CartId != user.Cart.Id)
        {
            throw new ValidationErrorsException(new List<string> { "Acesso negado ao item do carrinho." });
        }

        
        var product = await _productRepository.GetById(cartItem.ProductId);
        if (product == null) 
        {
            throw new ValidationErrorsException(new List<string> { "Produto associado não encontrado." });
        }

        
        if (product.StockQuantity < request.Quantity)
        {
            throw new ValidationErrorsException(new List<string> { "Estoque insuficiente para a quantidade solicitada." });
        }

        cartItem.Quantity = request.Quantity;

        await _cartItemRepository.Update(cartItem);
        return new ResponseCartItemJson
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity
        };
    }

    private void Validate(RequestUpdateCartItemJson request)
    {
        var validator = new UpdateItemValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationErrorsException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}