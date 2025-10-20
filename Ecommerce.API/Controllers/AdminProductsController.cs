using Ecommerce.Application.UseCases.Products.Create;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Policy = "AdminOnly")]
public class AdminProductsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseCreatedProductJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProduct(
        [FromServices] CreateProductUseCase useCase,
        [FromBody] RequestCreateProductJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}