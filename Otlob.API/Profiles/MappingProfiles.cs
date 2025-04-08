using AutoMapper;
using Otlob.API.DTOs;
using Otlob.API.Helpers;
using Otlob.Core.Models;

namespace Otlob.API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Configure the Mappings
            // Mapping [Source: Product] to [Destination: ProductDto]
            CreateMap<Product, ProductDto>()
                // Provide Mapping Configuration of ProductType Navigation Property and ProductType.Name Property
                .ForMember(t => t.ProductType, o => o.MapFrom(n => n.ProductType.Name))
                // Provide Mapping Configuration of ProductBrand Navigation Property and ProductBrand.Name Property
                .ForMember(b => b.ProductBrand, o => o.MapFrom(n => n.ProductBrand.Name))
                // Specify the PictureUrlResolver for the PictureUrl field in the ProductDto
                .ForMember(dto => dto.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
