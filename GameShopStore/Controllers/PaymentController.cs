using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Dtos.OrderDtos;
using GameShopStore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameShopStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountOrderPrice _countOrderPrice;
        private readonly ICreateCharge _createCharge;
        private readonly ISynchronizeBasket _synchronizeBasket;

        public PaymentController(IUnitOfWork unitOfWork, ICountOrderPrice countOrderPrice, ICreateCharge createCharge, ISynchronizeBasket synchronizeBasket)
        {
            _unitOfWork = unitOfWork;
            _countOrderPrice = countOrderPrice;
            _createCharge = createCharge;
            _synchronizeBasket = synchronizeBasket;
        }

        [HttpPost("charge")]
        public async Task<IActionResult> Charge([FromHeader(Name = "Stripe-Token")] string stripeToken, OrderInfoDto customerInfo)
        {
            var basketJson = HttpContext.Session.GetString("Basket");

            if (string.IsNullOrEmpty(basketJson))
            {
                return BadRequest("The Basket Cookie is empty");
            }

            var basketProductsCookie = JsonConvert.DeserializeObject<List<ProductFromBasketCookieDto>>(basketJson);

            if (!await _synchronizeBasket.Do(HttpContext.Session, basketProductsCookie) && !_synchronizeBasket.MissingStocks.Any())
            {
                return BadRequest("Stocks in Basket are not able to be assign to that Session");
            }

            var productsFromRepo = await _unitOfWork.StockOnHold.GetStockOnHoldWithProductForCharge(HttpContext.Session, basketProductsCookie);

            if (productsFromRepo.Count < 1)
            {
                return BadRequest("An error occured during retrieving data products from the Database");
            }

            decimal basketPrice = _countOrderPrice.Do(productsFromRepo);

            var basketForPaymentDto = _createCharge.Do(stripeToken, basketPrice, basketProductsCookie);

            var order = _unitOfWork.Order.ScaffoldOrderForCreation(customerInfo, basketForPaymentDto);

            await _unitOfWork.StockOnHold.DeleteRangeAsync(s => s.SessionId == HttpContext.Session.Id);

            _unitOfWork.Order.Add(order);

            if (await _unitOfWork.SaveAsync())
            {
                Response.Cookies.Delete("Basket");
                return Ok(201);
            }


            return BadRequest("Oops, something went wrong with saving your Order");
        }
    }
}
