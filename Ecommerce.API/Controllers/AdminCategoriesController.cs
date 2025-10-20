using Ecommerce.Application.UseCases.Categories.Create;
using Ecommerce.Application.UseCases.Categories.Delete;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/admin/categories")]
[Authorize(Policy = "AdminOnly")]
public class AdminCategoriesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCategory(
        [FromServices] CreateCategoryUseCase useCase,
        [FromBody] RequestCreateCategoryJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(
        [FromServices] DeleteCategoryUseCase useCase,
        [FromRoute] long id)
    {
        await useCase.Execute(id);

      
        return NoContent();
    }
}