using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Enums;
using GameShopStore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace GameShopStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BasketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAddStockBasket _addStockBasket;
        private readonly ICountOrderPrice _countOrderPrice;
        private readonly ITransferStockToBeOnHold _transferStockToBeOnHold;
        private readonly IDeleteFromBasket _deleteFromBasket;
        private readonly ISynchronizeBasket _synchronizeBasket;
        private readonly ITransferStockOnHoldWhenExpired _transferStockOnHoldWhenExpired;

        public BasketController(IUnitOfWork unitOfWork, IAddStockBasket addProductBasket, ICountOrderPrice countOrderPrice,
                        ITransferStockToBeOnHold transferStockToBeOnHold, IDeleteFromBasket deleteFromBasket, ISynchronizeBasket synchronizeBasket,
                        ITransferStockOnHoldWhenExpired transferStockOnHoldWhenExpired)
        {
            _unitOfWork = unitOfWork;
            _addStockBasket = addProductBasket;
            _countOrderPrice = countOrderPrice;
            _transferStockToBeOnHold = transferStockToBeOnHold;
            _deleteFromBasket = deleteFromBasket;
            _synchronizeBasket = synchronizeBasket;
            _transferStockOnHoldWhenExpired = transferStockOnHoldWhenExpired;
        }

        [HttpPost("add-stock/{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddStockToBasket(int id, [FromQuery] int stockQty)
        {
            if (stockQty < 1)
            {
                return BadRequest("Quantity that was passed is less then 1");
            }

            var stockToSubtract = await _unitOfWork.Stock.GetAsync(id);
            if (stockToSubtract == null)
            {
                return BadRequest("Stock for that Product don't exist");
            }

            if (stockToSubtract.Quantity < stockQty)
            {
                return BadRequest("Requested Stock Quantity is greater then Stock in Db");
            }
            var stockOnHoldToAdd = await _transferStockToBeOnHold.Do(StockTransferToOnHoldStockTypeEnum.OneWithUpdatingExpireTimeForBasketProducts, HttpContext.Session, stockToSubtract, stockQty);

            if (stockOnHoldToAdd == null)
            {
                return BadRequest("TODO Implement some better validation");
            }


            _addStockBasket.Do(HttpContext.Session, stockOnHoldToAdd);

            if (!await _unitOfWork.SaveAsync())
            {
                return BadRequest("Something went wrong during saving Stock To StockOnHold");
            }

            return Ok($"Added product {id} to basket TEST");
        }


        //Special note: Consider using _unitOfWork.Product.GetProductsForBasketAsync(basketCookie); sinc using Stock to get Product info can cause many Joins and it will badly affect the performance
        //!!OR we can use Refactor _unitOfWork.Stock.GetStockWithProductForBasket() to not use that many Joins
        [HttpGet("get-basket")]
        [ProducesResponseType(typeof(BasketDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basketJson = HttpContext.Session.GetString("Basket");
            if (string.IsNullOrEmpty(basketJson))
            {
                return NotFound();
            }

            var basketCookie = JsonConvert.DeserializeObject<List<ProductFromBasketCookieDto>>(basketJson);


            var stocksWithProductsFromRepo = await _unitOfWork.Stock.GetStockWithProductForBasket(basketCookie);

            decimal priceBasket = _countOrderPrice.Do(stocksWithProductsFromRepo);

            var basketToReturn = new BasketDto()
            {
                PriceBasket = priceBasket,
                ProdcutsBasket = stocksWithProductsFromRepo
            };

            return Ok(basketToReturn);


        }

        [HttpDelete("delete-stock/{stockId}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteStockFromBasket(int stockId)
        {
            var basketJson = HttpContext.Session.GetString("Basket");

            if (string.IsNullOrEmpty(basketJson))
            {
                return BadRequest("Basket is empty, there is no elements to be cleared");
            }

            var basketCookie = JsonConvert.DeserializeObject<List<ProductFromBasketCookieDto>>(basketJson);

            if (!await _deleteFromBasket.Do(HttpContext.Session, basketCookie, stockId))
            {
                return BadRequest("Something went wrong during removing StockOnHold");
            }

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Basket")))
            {
                Response.Cookies.Delete("Basket");
            }

            return NoContent();
        }


        [HttpPost("synchronize-basket")]
        [ProducesResponseType(typeof(List<NotEnoughStockInfoDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> EnsureThatBasketProductsAreInStockOnHold()
        {
            var basketJson = HttpContext.Session.GetString("Basket");
            if (string.IsNullOrEmpty(basketJson))
            {
                return NotFound();
            }

            var basketCookie = JsonConvert.DeserializeObject<List<ProductFromBasketCookieDto>>(basketJson);

            if (await _synchronizeBasket.Do(HttpContext.Session, basketCookie))
            {
                if (!await _unitOfWork.SaveAsync())
                {
                    return BadRequest("Something went wrong during saving");
                }
            }

            if (_synchronizeBasket.MissingStocks.Any())
            {
                return BadRequest(_synchronizeBasket.MissingStocks);
            }

            return Ok();


        }

        [HttpDelete("clear")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ClearBasket()
        {

            if (!await _unitOfWork.StockOnHold.SetExpiredForStocksOnHoldAsync(HttpContext.Session))
            {
                return NotFound();
            }

            if (!await _unitOfWork.SaveAsync())
            {
                return BadRequest("Something went wrong saving StocksOnHold expire time");
            }

            if (!await _transferStockOnHoldWhenExpired.Do())
            {
                return BadRequest("Something went wrong during transfering StockOnHold to Stock");
            }



            Response.Cookies.Delete("Basket");
            return NoContent();
        }
    }
}
