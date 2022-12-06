using GameShopStore.Application.Helpers;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.BasketDtos;
using GameShopStore.Core.Enums;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Basket
{
    public class BasketSyncronize : ISynchronizeBasket
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransferStockToBeOnHold _transferStockToBeOnHold;
        private readonly IOptions<BasketSettings> _basketSettings;

        public List<NotEnoughStockInfoDto> MissingStocks { get; private set; }

        public BasketSyncronize(IUnitOfWork unitOfWork, ITransferStockToBeOnHold transferStockToBeOnHold, IOptions<BasketSettings> basketSettings)
        {
            _unitOfWork = unitOfWork;
            _transferStockToBeOnHold = transferStockToBeOnHold;
            _basketSettings = basketSettings;

            MissingStocks = new List<NotEnoughStockInfoDto>();
        }

        public async Task<bool> Do(ISession session, List<ProductFromBasketCookieDto> basketCookie)
        {
            if (basketCookie == null || !basketCookie.Any())
            {
                throw new ArgumentException("passed product list from basket that is null or empty");
            }

            var stocksOnHoldForThatBasket = await _unitOfWork.StockOnHold.FindAllAsync(s => s.SessionId == session.Id);
            var stockIdsFromStockOnHold = stocksOnHoldForThatBasket.Select(s => s.StockId).ToList();
            foreach (var product in basketCookie)
            {
                if (stockIdsFromStockOnHold.Contains(product.StockId))
                {
                    var stockOnHoldThatIsAssignedToThatBasekt = stocksOnHoldForThatBasket.FirstOrDefault(s => s.StockId == product.StockId);

                    stockOnHoldThatIsAssignedToThatBasekt.ExpireTime = DateTime.Now.AddMinutes(_basketSettings.Value.StockOnHoldMinutesExpired);
                }
                else
                {
                    var stockToAttemptToGetForBasket = await _unitOfWork.Stock.FindWithProductAsync(s => s.Id == product.StockId);

                    if (stockToAttemptToGetForBasket == null)
                    {
                        throw new ArgumentException("StockId from Basket not exist in Stock Entity");
                    }

                    if (stockToAttemptToGetForBasket.Quantity < product.StockQty)
                    {
                        AddMissingStockToList(stockToAttemptToGetForBasket);
                    }
                    else
                    {
                        await _transferStockToBeOnHold.Do(StockTransferToOnHoldStockTypeEnum.One, session, stockToAttemptToGetForBasket, product.StockQty);
                    }

                }
            }

            if (_unitOfWork.IsAnyEntityAdded() || _unitOfWork.IsAnyEntityModified())
            {
                return true;
            }

            return false;

        }

        private void AddMissingStockToList(Stock stockThatHasNotEnoughQty)
        {
            var missingStock = new NotEnoughStockInfoDto()
            {
                StockId = stockThatHasNotEnoughQty.Id,
                ProductName = stockThatHasNotEnoughQty.Product.Name,
                AvailableStockQty = stockThatHasNotEnoughQty.Quantity
            };

            MissingStocks.Add(missingStock);
        }
    }
}
