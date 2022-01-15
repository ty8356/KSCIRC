using System;
using System.Net;

namespace KSCIRC.Models.ErrorHandling.Exceptions
{
    [StatusCode(HttpStatusCode.Unauthorized)]
    public class HttpUnauthorizedException : Exception
    {
        public HttpUnauthorizedException()
        {
        }

        public HttpUnauthorizedException(string message)
            : base(message)
        {
        }

        public HttpUnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}