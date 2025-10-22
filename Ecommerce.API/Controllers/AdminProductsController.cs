using Ecommerce.Application.UseCases.Products.Create;
using Ecommerce.Application.UseCases.Products.Delete;
using Ecommerce.Application.UseCases.Products.Update;
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

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] 
    public async Task<IActionResult> UpdateProduct(
        [FromServices] UpdateProductUseCase useCase,
        [FromRoute] long id, 
        [FromBody] RequestCreateProductJson request) 
    {
        await useCase.Execute(id, request);
        return NoContent();
    }

    [HttpDelete("{id:long}")] 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] 
    public async Task<IActionResult> DeleteProduct(
        [FromServices] DeleteProductUseCase useCase,
        [FromRoute] long id) 
    {
        await useCase.Execute(id);
        return NoContent(); 
    }
}