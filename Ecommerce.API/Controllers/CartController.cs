// Ecommerce.API/Controllers/CartController.cs
using Ecommerce.Application.UseCases.Carts;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/cart/items")]
[Authorize] 
public class CartController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseCartItemJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItemToCart(
        [FromServices] AddCartItemUseCase useCase,
        [FromBody] RequestAddItemToCartJson request)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var response = await useCase.Execute(userEmail!, request);

        return Created(string.Empty, response);
    }
}