using Ecommerce.Application.UseCases.UserUseCase.GetAll;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/admin/users")] 
[Authorize(Policy = "AdminOnly")] 
public class AdminUsersController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseAllUsersJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(
        [FromServices] GetAllUsersUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
}