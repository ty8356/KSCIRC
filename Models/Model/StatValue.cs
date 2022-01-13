using System;
using System.Collections.Generic;

#nullable disable

namespace KSCIRC.Models.Model
{
    public partial class StatValue
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

        public virtual Gene Gene { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
