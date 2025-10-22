using Ecommerce.Application.UseCases.Carts;
using Ecommerce.Application.UseCases.Carts.Clear;
using Ecommerce.Application.UseCases.Carts.DeleteItem;
using Ecommerce.Application.UseCases.Carts.UpdateItem;
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

    [HttpGet]
    [ProducesResponseType(typeof(ResponseCartJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCart(
        [FromServices] GetCartUseCase useCase)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var response = await useCase.Execute(userEmail!);
        return Ok(response);
    }

    [HttpPut("items/{itemId:long}")] 
    [ProducesResponseType(typeof(ResponseCartItemJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateCartItemQuantity(
        [FromServices] UpdateCartItemUseCase useCase,
        [FromRoute] long itemId, 
        [FromBody] RequestUpdateCartItemJson request)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var response = await useCase.Execute(userEmail!, itemId, request);
        return Ok(response);
    }

    [HttpDelete("items/{itemId:long}")] 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)] 
    public async Task<IActionResult> DeleteCartItem(
        [FromServices] DeleteCartItemUseCase useCase,
        [FromRoute] long itemId) 
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        await useCase.Execute(userEmail!, itemId);

        return NoContent(); 
    }
    [HttpDelete] 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearCart(
        [FromServices] ClearCartUseCase useCase)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        await useCase.Execute(userEmail!);

        return NoContent();     
    }
}