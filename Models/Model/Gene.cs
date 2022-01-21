using System;
using System.Collections.Generic;

namespace KSCIRC.Models.Model
{
    public partial class Gene
    {
        public Gene()
        {
            StatValues = new HashSet<StatValue>();
        }

        public int Id { get; set; }
        public string EnsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<StatValue> StatValues { get; set; }
    }
}
