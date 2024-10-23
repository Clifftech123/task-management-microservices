using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using UserService.API.Domain.Contracts;

namespace UserService.API.Infrastructure.Exceptions
{


    public class GlobalExceptionHandler : IExceptionHandler
    {


        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }


        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {

            _logger.LogError(
                $"An error occurred while processing your request: {exception.Message}");


            var errorResponse = new ErrorResponse
            {
                Message = exception.Message
            };

            // Determine the status code and title based on the type of exception.
            switch (exception)
            {
                case BadHttpRequestException:
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Title = exception.GetType().Name;
                    break;

                default:
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Title = "Internal Server Error";
                    break;
            }


            httpContext.Response.StatusCode = errorResponse.StatusCode;


            await httpContext
                .Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            // Return true to indicate that the exception was handled.
            return true;
        }
    }
}
