using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KSCIRC.Models.ErrorHandling
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class StatusCodeAttribute : Attribute
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public StatusCodeAttribute(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
