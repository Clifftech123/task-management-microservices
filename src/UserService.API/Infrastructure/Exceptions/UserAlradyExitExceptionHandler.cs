using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using UserService.API.Domain.Contracts;

namespace UserService.API.Infrastructure.Exceptions
{

    public class UserAlradyExitExceptionHandler(ILogger<UserAlradyExitExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError($"Exception caught: {exception.Message}");

            ErrorResponse errorResponse;

            // Handle UserNotFoundException
            if (exception is UserNotFoundException userNotFoundException)
            {
                logger.LogError($"User not found: {userNotFoundException.Message}");
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Title = exception.GetType().Name,
                    Message = userNotFoundException.Message
                };

                httpContext.Response.StatusCode = errorResponse.StatusCode;
                await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
                return true;
            }

            // Handle UserAlreadyExistsException
            if (exception is UserAlreadyExistsException userAlreadyExistsException)
            {
                logger.LogError($"User already exists: {userAlreadyExistsException.Message}");
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Conflict, // 409 Conflict
                    Title = exception.GetType().Name,
                    Message = userAlreadyExistsException.Message
                };

                httpContext.Response.StatusCode = errorResponse.StatusCode;
                await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
                return true;
            }

            return false; // Not a handled exception
        }
    }
}
