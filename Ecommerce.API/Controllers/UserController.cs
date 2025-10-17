// Ecommerce.API/Controllers/UserController.cs
using Ecommerce.Application.UseCases.User.Register;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public IActionResult Register(
        [FromServices] RegisterUserUseCase useCase, // Recebe o UseCase por injeção de dependência
        [FromBody] RequestRegisterUserJson request)
    {
        var response = useCase.Execute(request);

        return Created(string.Empty, response);
    }
}