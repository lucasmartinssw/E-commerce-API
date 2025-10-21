using Ecommerce.Application.UseCases.Products.GetAllPaged;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponsePagedData<ResponseProductSummaryJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPagedProducts(
        [FromServices] GetAllPagedProductsUseCase useCase,
        [FromQuery] RequestProductQuery query)
    {
        var response = await useCase.Execute(query);
        return Ok(response);
    }
}