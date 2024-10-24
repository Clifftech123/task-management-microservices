using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using UserService.API.Domain.Contracts;

namespace UserService.API.Infrastructure.Exceptions
{

    public class UserNotFoundExceptionHandler(ILogger<UserNotFoundExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError($"Exception caught: {exception.Message}");

            // Check if the exception is of type UserNotFoundException (your custom exception)
            if (exception is not UserNotFoundException userNotFoundException)
            {
                return false; // Not the exception we are handling
            }

            // Log the detailed user not found exception
            logger.LogError($"User not found: {userNotFoundException.Message}");

            // Prepare an error response
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Title =  exception.GetType().Name, 
                Message = userNotFoundException.Message 
            };

            // Set the response status code
            httpContext.Response.StatusCode = errorResponse.StatusCode;

            // Write the response as JSON
            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true; // Exception handled successfully
        }
    }
}

