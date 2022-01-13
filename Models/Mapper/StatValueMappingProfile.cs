using AutoMapper;
using KSCIRC.Models.Model;
using KSCIRC.Models.ResponseModel;

namespace KSCIRC.Models.Mapper
{
    public class StatValueMappingProfile : Profile
    {
        public StatValueMappingProfile()
        {
            CreateMap<StatValue, StatValueResponseModel>()
                ;
        }
    }
}