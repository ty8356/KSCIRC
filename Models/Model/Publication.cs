using System;
using System.Collections.Generic;

namespace KSCIRC.Models.Model
{
    public partial class Publication
    {
        public Publication()
        {
            StatValues = new HashSet<StatValue>();
        }

        public int Id { get; set; }
        public int? Pmid { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public virtual ICollection<StatValue> StatValues { get; set; }
    }
}
