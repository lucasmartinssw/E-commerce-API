using Ecommerce.Application.UseCases.UserUseCase.Login;
using Ecommerce.Application.UseCases.UserUseCase.Register;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] RegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] LoginUseCase useCase,
        [FromBody] RequestLoginUserJson request)
    {
        try
        {
            var response = await useCase.Execute(request);
            return Ok(response);
        }
        catch (ValidationErrorsException ex)
        {
            return Unauthorized(new { errors = ex.ErrorMessages });
        }
    }
}