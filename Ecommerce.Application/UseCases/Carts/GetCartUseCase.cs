using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Carts;

public class GetCartUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;

    public GetCartUseCase(IUserRepository userRepository, ICartRepository cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task<ResponseCartJson> Execute(string userEmail)
    {
        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não autenticado." });
        }

        var cart = await _cartRepository.GetByUserId(user.Id);

        if (cart == null)
        {
            return new ResponseCartJson { Id = 0, Items = new List<ResponseCartItemDetailJson>(), TotalAmount = 0 };
        }

        var responseItems = cart.Items.Select(item => new ResponseCartItemDetailJson
        {
            Id = item.Id,
            Quantity = item.Quantity,
            Product = new ResponseProductSummaryJson 
            {
                Id = item.Product.Id,
                Name = item.Product.Name,
                Price = item.Product.Price,
                ImageFilename = item.Product.ImageFilename,
                CategoryName = item.Product.Category.Name 
            }
        }).ToList();
 
        decimal totalAmount = responseItems.Sum(item => item.Quantity * item.Product.Price);

        return new ResponseCartJson
        {
            Id = cart.Id,
            Items = responseItems,
            TotalAmount = totalAmount
        };
    }
}