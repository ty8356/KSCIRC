using System;
using System.Collections.Generic;

namespace KSCIRC.Models.ResponseModel
{
    public class PublicationResponseModel
    {
        public int Id { get; set; }
        public int? Pmid { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
