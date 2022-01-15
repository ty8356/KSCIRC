using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KSCIRC.Models.ErrorHandling;
using KSCIRC.Models.ErrorHandling.Exceptions;

namespace KSCIRC.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AggregateException ae)
            {
                await HandleAggregateExceptionAsync(context, ae);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = exception.GetType().GetCustomAttribute<StatusCodeAttribute>()?.HttpStatusCode ?? HttpStatusCode.InternalServerError;

            var message = JsonConvert.SerializeObject(new
            {
                error = exception.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(message);
        }


        private static Task HandleAggregateExceptionAsync(HttpContext context, AggregateException exception)
        {
            var code = HttpStatusCode.InternalServerError;
            StringBuilder errorMessage = new StringBuilder();

            exception.Handle((ex) =>
             {
                 if (ex is UnauthorizedAccessException)
                 {
                     code = HttpStatusCode.Unauthorized;
                 }
                 else if (ex is HttpNotFoundException)
                 {
                     code = HttpStatusCode.NotFound;
                 }
                 
                 errorMessage.Append(ex.Message + "; ");                 

                 return true;
             });

            var message = JsonConvert.SerializeObject(new { error = errorMessage.ToString() });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(message);
        }
    }
}
