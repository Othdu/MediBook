using MediBook.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace MediBook.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BookingConflictException ex)
            {
                await WriteError(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (SlotUnavailableException ex)
            {
                await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidStatusTransitionException ex)
            {
                await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                await WriteError(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }


        private static async Task WriteError(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
        }
    }
}