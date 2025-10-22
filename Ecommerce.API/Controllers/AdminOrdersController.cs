using Ecommerce.Application.UseCases.Orders.GetAllAdmin;
using Ecommerce.Application.UseCases.Orders.UpdateStatus;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/admin/orders")] 
[Authorize(Policy = "AdminOnly")] 
public class AdminOrdersController : ControllerBase
{
    [HttpGet] 
    [ProducesResponseType(typeof(ResponseAllAdminOrdersJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders(
        [FromServices] GetAllOrdersAdminUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }

    [HttpPut("{id:long}/status")] 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] 
    public async Task<IActionResult> UpdateOrderStatus(
        [FromServices] UpdateOrderStatusUseCase useCase,
        [FromRoute] long id, 
        [FromBody] RequestUpdateOrderStatusJson request) 
    {
        await useCase.Execute(id, request);
        return NoContent(); 
    }
}