using Ecommerce.Application.UseCases.Orders.Checkout;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/orders")] 
[Authorize] 
public class OrdersController : ControllerBase
{
    [HttpPost] 
    [ProducesResponseType(typeof(ResponseOrderJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder(
        [FromServices] CheckoutUseCase useCase,
        [FromBody] RequestCreateOrderJson request)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var response = await useCase.Execute(userEmail!, request);

        return Created($"/api/orders/{response.Id}", response);
    }
}