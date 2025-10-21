using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Application.UseCases.Carts;

public class AddCartItemUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public AddCartItemUseCase(
        IUserRepository userRepository,
        IProductRepository productRepository,
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository)
    {
        _userRepository = userRepository;
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<ResponseCartItemJson> Execute(string userEmail, RequestAddItemToCartJson request)
    {
        Validate(request);

        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
            throw new ValidationErrorsException(new List<string> { "Usuário não autenticado." });

        var product = await _productRepository.GetById(request.ProductId);
        if (product == null)
            throw new ValidationErrorsException(new List<string> { "Produto não encontrado." });

        
        var cart = await _cartRepository.GetByUserId(user.Id);
        if (cart == null)
        {
            cart = new Cart { UserId = user.Id };
            await _cartRepository.Add(cart);
        }

        
        var existingItem = await _cartItemRepository.GetByCartAndProduct(cart.Id, request.ProductId);

        if (existingItem != null)
        {
            
            existingItem.Quantity += request.Quantity;

           
            if (product.StockQuantity < existingItem.Quantity)
                throw new ValidationErrorsException(new List<string> { "Estoque insuficiente." });

            await _cartItemRepository.Update(existingItem);
            return new ResponseCartItemJson { Id = existingItem.Id, ProductId = existingItem.ProductId, Quantity = existingItem.Quantity };
        }
        else
        {
            
            if (product.StockQuantity < request.Quantity)
                throw new ValidationErrorsException(new List<string> { "Estoque insuficiente." });

            
            var newItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            await _cartItemRepository.Add(newItem);
            return new ResponseCartItemJson { Id = newItem.Id, ProductId = newItem.ProductId, Quantity = newItem.Quantity };
        }
    }

    private void Validate(RequestAddItemToCartJson request)
    {
        var validator = new AddItemValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ValidationErrorsException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}