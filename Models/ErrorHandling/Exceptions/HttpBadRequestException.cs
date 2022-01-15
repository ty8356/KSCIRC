using System;
using System.Net;

namespace KSCIRC.Models.ErrorHandling.Exceptions
{
    [StatusCode(HttpStatusCode.BadRequest)]
    public class HttpBadRequestException : Exception
    {
        public HttpBadRequestException()
        {
        }

        public new object Data { get; set; }

        public HttpBadRequestException(string message)
            : base(message)
        {
        }

        public HttpBadRequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}