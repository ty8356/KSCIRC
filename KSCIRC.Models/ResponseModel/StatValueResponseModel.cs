using System;
using System.Collections.Generic;

namespace KSCIRC.Models.ResponseModel
{
    public class StatValueResponseModel
    {
        public int Id { get; set; }
        public int GeneId { get; set; }
        public int PublicationId { get; set; }
        public int? DaysPostInjury { get; set; }
        public decimal? InputValue { get; set; }
        public decimal? InputQvalue { get; set; }
        public decimal? ImmunoprecipitateValue { get; set; }
        public decimal? ImmunoprecipitateQvalue { get; set; }
        public decimal? EnrichmentValue { get; set; }
        public decimal? EnrichmentQvalue { get; set; }
        public decimal? InteractionValue { get; set; }
        public decimal? InteractionQvalue { get; set; }
    }
}
