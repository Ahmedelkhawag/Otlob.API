using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.API.DTOs;
using Otlob.API.Errors;
using Otlob.API.Helpers;
using Otlob.Core.Models;
using Otlob.Core.Repositories;
using Otlob.Core.Specifications;

namespace Otlob.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _TypesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> brandRepo, IGenericRepository<ProductType> TypesRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _brandRepo = brandRepo;
            _TypesRepo = TypesRepo;
            _mapper = mapper; // Inject IMapper 
        }

        #region EndPoints

        #region GET: BaseUrl/api/Product
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts([FromQuery] ProductSpecificationParams productParams)
        {
            var specification = new ProductWithBrandAndTypeSpecification(productParams);

            var products = await _productRepo.GetAllWithSpecificationAsync(specification);

            var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

            var paginationResponse = new PaginationResponse<ProductDto>
                (productParams.pageNumber, productParams.PageSize, _productRepo.GetAllAsync().Result.Count, mappedProducts);

            return Ok(paginationResponse);
        }
        #endregion

        #region GET: BaseUrl/api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var specification = new ProductWithBrandAndTypeSpecification(id);
            var product = await _productRepo.GetEntityWithSpecificationAsync(specification);

            if (product is null)
                return NotFound(new ErrorResponse(404));

            var mappedProduct = _mapper.Map<Product, ProductDto>(product);

            return Ok(mappedProduct);
        }
        #endregion

        #region GET: BaseUrl/api/Product/Types
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductsTypes(string? sort)
        {
            var specification = new ProductTypesSpecification(sort);

            var productTypes = await _TypesRepo.GetAllWithSpecificationAsync(specification);

            return Ok(productTypes);
        }
        #endregion

        #region GET: BaseUrl/api/Product/Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(string? sort)
        {
            var specification = new ProductBrandsSpecification(sort);

            var productBrands = await _brandRepo.GetAllWithSpecificationAsync(specification);

            return Ok(productBrands);
        }
        #endregion

        #endregion
    }
}
