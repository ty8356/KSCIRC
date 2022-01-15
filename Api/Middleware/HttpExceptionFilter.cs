using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using KSCIRC.Models.ErrorHandling;

namespace KSCIRC.Api.Middleware
{
    internal class HttpExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var httpException = context.Exception as HttpException;

            if (httpException != null)
            {
                //handle httpexception
                object obj = new { Message = httpException.Message };

                var res = new ObjectResult(obj);
                res.StatusCode = httpException.StatusCode;
                context.Result = res;
                context.ExceptionHandled = true;
            }
            else
            {
                if (context.Exception.InnerException != null)
                {
                    //handle inner exception for the front end boyz
                    object obj = new
                    {
                        Message = context.Exception.InnerException.Message,
                        StackTrace = context.Exception.InnerException.StackTrace
                    };

                    var res = new ObjectResult(obj);
                    res.StatusCode = StatusCodes.Status500InternalServerError;

                    context.Result = res;
                    context.ExceptionHandled = true;
                }
            }
        }
    }
}