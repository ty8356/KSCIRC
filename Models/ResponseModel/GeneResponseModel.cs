using System.Collections.Generic;

namespace KSCIRC.Models.ResponseModel
{
    public class GeneResponseModel
    {
        public int Id { get; set; }
        public string EnsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<StatValueResponseModel> StatValues { get; set; }
    }
}
