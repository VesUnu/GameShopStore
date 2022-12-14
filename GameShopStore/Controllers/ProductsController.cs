using AutoMapper;
using GameShopStore.Application.Helpers;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.ProductDtos;
using GameShopStore.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStore.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [TransferStockOnHoldWhenExpire]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetProductsForSearching([FromQuery] ProductParameters productParameters)
        {
            if (productParameters.PageNumber < 1 || productParameters.PageSize < 1)
            {
                return BadRequest("The PageNumber or PageSize is less than 1");
            }

            var products = await _unitOfWork.Product.GetProductsForSearchingAsync(productParameters);

            if (products == null)
            {
                return NotFound();
            }
            else if (products.PageSize < 1 || products.TotalCount < 0 || products.TotalPages < 1 || products.CurrentPage < 1)
            {
                return BadRequest("Pagination parameters wasn't set properly");
            }

            var productToReturn = _mapper.Map<IEnumerable<ProductSearchDto>>(products);

            Response.AddPagination(products.CurrentPage, products.PageSize,
                            products.TotalCount, products.TotalPages);

            return Ok(productToReturn);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }


            return Ok(product);
        }
    }
}
