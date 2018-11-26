using AutoMapper;
using Challenge1.Data.Dtos;
using Challenge1.Features.Models;

namespace Challenge1
{
    /// <summary>
    /// Mapping configurations for mapping database dtos to http models, and vice versa.
    /// </summary>
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            this.CreateMap<MessageRequestModel, HashDto>()
                .ForMember(d => d.Hash, opt => opt.MapFrom(m => m.Digest))
                .ForMember(d => d.Value, opt => opt.MapFrom(m => m.Message));
            this.CreateMap<HashDto, MessageResponseModel>()
                .ForMember(m => m.Message, opt => opt.MapFrom(d => d.Value));
        }
    }
}
