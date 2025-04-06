using AutoMapper;
using Otlob.API.DTOs;
using Otlob.Core.Models;

namespace Otlob.API.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration; // Inject IConfiguration Service To get "ApiBaseUrl" from appsettings.json
        }



        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            return !string.IsNullOrEmpty(source.PictureUrl) ? $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}" : string.Empty;
        }
    }
}
