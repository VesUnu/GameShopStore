using GameShopStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class StockController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-by-productId/{id}")]
        public async Task<IActionResult> GetStockForProduct(int id)
        {
            var stock = await _unitOfWork.Stock.GetByProductId(id);
            if (stock == null)
            {
                return NotFound();
            }
            var stockToReturn = new
            {
                StockId = stock.Id,
                Qty = stock.Quantity
            };

            return Ok(stockToReturn);
        }
    }
}
