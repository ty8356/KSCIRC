using System;
using System.Net;

namespace KSCIRC.Models.ErrorHandling.Exceptions
{
    [StatusCode(HttpStatusCode.NotFound)]
    public class HttpNotFoundException : Exception
    {
        public HttpNotFoundException()
        {
        }

        public HttpNotFoundException(string message)
            : base(message)
        {
        }

        public HttpNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}