using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zModelLayer;

namespace zAutoMapperRepository
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Product, ProductContainer>()
            .ForMember(dest => dest.ProductContainer_Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductContainer_Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ProductContainer_description, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.ProductContainer_price, opt => opt.MapFrom(src => string.Format("{0:C0}", src.price)))
            .ForMember(dest => dest.categoryContainers, opt => opt.MapFrom(src => src.categories));
            CreateMap<ProductContainer, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductContainer_Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductContainer_Name))
            .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.ProductContainer_description))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => decimal.Parse(src.ProductContainer_price, NumberStyles.Currency)))
            .ForMember(dest => dest.categories , opt => opt.MapFrom(src => src.categoryContainers));
        }
    }
}
