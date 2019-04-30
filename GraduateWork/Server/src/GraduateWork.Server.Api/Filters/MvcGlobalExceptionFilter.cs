using System;
using System.Net;
using GraduateWork.Server.Models.CustomExceptions;
using GraduateWork.Server.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraduateWork.Server.Api.Filters
{
    /// <summary>
    /// Filter for handling exceptions on server side int HttpContext live sickle.
    /// </summary>
    public class MvcGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<MvcGlobalExceptionFilter> _logger;
        private readonly bool _showErrorDetails;

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> instance.</param>
        /// <param name="config"><see cref="IConfiguration"/> instance.</param>
        public MvcGlobalExceptionFilter(ILogger<MvcGlobalExceptionFilter> logger, IConfiguration config)
        {
            _logger = logger;
            _showErrorDetails = config.GetValue<bool?>("DumpExceptionInResponse") ?? false;
        }

        /// <summary>
        /// Method for handle exception.
        /// </summary>
        /// <param name="context"><see cref="ExceptionContext"/> instance.</param>
        public void OnException(ExceptionContext context)
        {
            var (error, statusCode) = PrepareResponseForException(context.Exception);

            context.ExceptionHandled = true;

            context.Result = new ObjectResult(error)
            {
                StatusCode = (int)statusCode
            };
        }

        private (ResponseError, HttpStatusCode) PrepareResponseForException(Exception exception)
        {
            ResponseError error;
            HttpStatusCode statusCode;

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    error = new ResponseError(notFoundException);
                    break;
                case OperationCanceledException operationCanceledException:
                    statusCode = HttpStatusCode.RequestTimeout;
                    error = new ResponseError
                    {
                        Message = "Task was cancelled",
                        Errors = _showErrorDetails ? operationCanceledException.ToString() : null
                    };
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    error = new ResponseError
                    {
                        Message = "Internal Server Error",
                        Errors = _showErrorDetails ? exception.ToString() : null
                    };

                    _logger?.LogCritical(exception, $"REST API Internal Server Error: {exception.Message}");

                    break;
            }

            return (error, statusCode);
        }
    }
}
