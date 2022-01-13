using AutoMapper;
using KSCIRC.Models.Model;
using KSCIRC.Models.ResponseModel;

namespace KSCIRC.Models.Mapper
{
    public class GeneMappingProfile : Profile
    {
        public GeneMappingProfile()
        {
            CreateMap<Gene, GeneResponseModel>()
                ;
        }
    }
}