using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Basket
{
    public class StockDeletedFromBasket : IDeleteFromBasket
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockDeletedFromBasket(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Do(ISession session, List<ProductFromBasketCookieDto> basketCookie, int stockIdToDelete)
        {
            var deletingProduct = basketCookie.FirstOrDefault(b => b.StockId == stockIdToDelete);

            if (deletingProduct == null)
            {
                return false;
            }

            var stockOnHold = await _unitOfWork.StockOnHold.FindAsync(s => s.SessionId == session.Id && s.StockId == deletingProduct.StockId);
            if (stockOnHold != null)
            {
                var stockToIncreaseQty = await _unitOfWork.Stock.GetAsync(stockOnHold.StockId);
                stockToIncreaseQty.Quantity += stockOnHold.StockQty;

                _unitOfWork.StockOnHold.Delete(stockOnHold);

                if (!await _unitOfWork.SaveAsync())
                {
                    return false;
                }
            }

            basketCookie.Remove(deletingProduct);

            string basketJson;
            if (!basketCookie.Any())
            {
                basketJson = string.Empty;
            }
            else
            {
                basketJson = JsonConvert.SerializeObject(basketCookie);

            }

            session.SetString("Basket", basketJson);

            return true;
        }
    }
}
