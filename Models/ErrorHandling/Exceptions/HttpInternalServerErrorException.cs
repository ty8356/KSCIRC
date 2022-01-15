using System;
using System.Net;

namespace KSCIRC.Models.ErrorHandling.Exceptions
{
    [StatusCode(HttpStatusCode.InternalServerError)]
    public class HttpInternalServerErrorException : Exception
    {
        public HttpInternalServerErrorException()
        {
        }

        public HttpInternalServerErrorException(string message)
            : base(message)
        {
        }

        public HttpInternalServerErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}