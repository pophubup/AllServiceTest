using AutoMapper;
using System;
using zModelLayer;

namespace zAutoMapperRepository
{
    public class ContainerMapperProfile : Profile
    {
        public ContainerMapperProfile()
        {
            CreateMap<MyModel0, Container>()
                .ForMember(dest => dest.ContainerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.productContainers, opt => opt.MapFrom(src => src.products));

            CreateMap<Container, MyModel0>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContainerId))
                .ForMember(dest => dest.products, opt => opt.MapFrom(src => src.productContainers));
        }
    }
}
