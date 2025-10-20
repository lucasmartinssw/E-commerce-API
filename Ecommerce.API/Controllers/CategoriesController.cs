using Ecommerce.Application.UseCases.Categories.GetAll;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseAllCategoriesJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCategories(
        [FromServices] GetAllCategoriesUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
}