using AutoMapper;
using Robotic.Folklift.Domain.Entities;
using Robotic.Folklift.Application.Dtos;

namespace Robotic.Folklift.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Forklift, ForkliftDto>();
        }
    }
}
