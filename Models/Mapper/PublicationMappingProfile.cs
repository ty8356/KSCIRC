using AutoMapper;
using KSCIRC.Models.Model;
using KSCIRC.Models.ResponseModel;

namespace KSCIRC.Models.Mapper
{
    public class PublicationMappingProfile : Profile
    {
        public PublicationMappingProfile()
        {
            CreateMap<Publication, PublicationResponseModel>()
                ;
        }
    }
}