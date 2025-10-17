// Ecommerce.API/Middleware/ExceptionHandlingMiddleware.cs
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Text.Json;

namespace Ecommerce.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Tenta executar a próxima etapa do pipeline (o controller)
            await _next(context);
        }
        catch (Exception ex)
        {
            // Se uma exceção for capturada, chama nosso método para tratá-la
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Se for a nossa exceção de validação...
        if (exception is ValidationErrorsException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            // Escreve a lista de erros no corpo da resposta
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { errors = validationException.ErrorMessages }));
        }
        else
        {
            // Para qualquer outro tipo de erro, retorna um erro genérico
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Ocorreu um erro inesperado no servidor." }));
        }
    }
}