using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zModelLayer;

namespace zAutoMapperRepository
{
    public class CategoryMappProfile : Profile
    {
        public CategoryMappProfile()
        {
            CreateMap<Category, CategoryContainer>()
                .ForMember(dest => dest.CategoryContainer_Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryContainer_Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<CategoryContainer , Category>()
              .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.CategoryContainer_Id))
              .ForMember(dest => dest.Name , opt => opt.MapFrom(src => src.CategoryContainer_Name));
        }
    }
}
