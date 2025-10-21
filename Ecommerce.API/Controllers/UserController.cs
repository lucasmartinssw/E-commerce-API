using Ecommerce.Application.UseCases.UserUseCase.ChangePassword;
using Ecommerce.Application.UseCases.UserUseCase.GetProfile;
using Ecommerce.Application.UseCases.UserUseCase.Login;
using Ecommerce.Application.UseCases.UserUseCase.Register;
using Ecommerce.Application.UseCases.UserUseCase.UpdateProfile;
using Ecommerce.Application.UseCases.Addresses.AddByCep;
using Ecommerce.Application.UseCases.Addresses.GetAll;
using Ecommerce.Application.UseCases.Addresses.Update;
using Ecommerce.Application.UseCases.Addresses.Delete;
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] RegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPost]
    [AllowAnonymous]
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

    [HttpPost]
    [Route("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        // O cliente que deve excluir o token após receber esta resposta.
        return Ok(new { message = "Ok, entendi. Agora se vire e apague o token aí do seu lado." });
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile(
        [FromServices] GetUserProfileUseCase useCase)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var response = await useCase.Execute(userEmail!);
        return Ok(response);
    }

    [HttpPut("me")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMyProfile(
        [FromServices] UpdateUserProfileUseCase useCase,
        [FromBody] RequestUpdateUserProfileJson request)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        await useCase.Execute(userEmail!, request);
        return NoContent();
    }

    [HttpPut("me/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeMyPassword(
        [FromServices] ChangePasswordUseCase useCase,
        [FromBody] RequestChangePasswordJson request)
    {
        
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        await useCase.Execute(userEmail!, request);

        return NoContent();
    }

    [HttpPost("me/addresses/by-cep")]
    [ProducesResponseType(typeof(ResponseAddressJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMyAddressByCep(
    [FromServices] AddAddressByCepUseCase useCase,
    [FromBody] RequestAddAddressByCepJson request)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var response = await useCase.Execute(userEmail!, request);

        return Created($"/api/user/me/addresses/{response.Id}", response);
    }

    [HttpGet("me/addresses")] 
    [ProducesResponseType(typeof(ResponseAllAddressesJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyAddresses(
        [FromServices] GetAllAddressesUseCase useCase)
    {
        
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

       
        var response = await useCase.Execute(userEmail!);

        return Ok(response);
    }

    [HttpPut("me/addresses/{id:long}")] 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] 
    public async Task<IActionResult> UpdateMyAddress(
        [FromServices] UpdateAddressUseCase useCase,
        [FromRoute] long id, 
        [FromBody] RequestAddAddressJson request) 
    {
        
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        
        await useCase.Execute(userEmail!, id, request);

        return NoContent();
    }

    [HttpDelete("me/addresses/{id:long}")] 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)] 
    [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteMyAddress(
        [FromServices] DeleteAddressUseCase useCase,
        [FromRoute] long id)
    {
        
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        await useCase.Execute(userEmail!, id);

        return NoContent();
    }
}