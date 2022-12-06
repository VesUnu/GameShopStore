using GameShopStore.Application.Helpers;
using GameShopStore.Application.Interfaces;
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
    public class TransferToStockOnHold : ITransferStockToBeOnHold
    {
        private IUnitOfWork _unitOfWork;
        private readonly IOptions<BasketSettings> _basketOptions;

        public TransferToStockOnHold(IUnitOfWork unitOfWork, IOptions<BasketSettings> basketOptions)
        {
            _unitOfWork = unitOfWork;
            _basketOptions = basketOptions;
        }

        public async Task<StockOnHold> Do(StockTransferToOnHoldStockTypeEnum transferType, ISession session, Stock stockToSubtract, int stockQty)
        {
            if (stockToSubtract == null)
            {
                return null;
            }

            if (stockToSubtract.Quantity < stockQty)
            {
                return null;
            }

            await ChangeExpireTimeForExistingStocksOnHold(transferType, session);

            var stockOnHoldToBeAdded = await _unitOfWork.StockOnHold.FindAsync(s => s.SessionId == session.Id && s.StockId == stockToSubtract.Id);

            if (stockOnHoldToBeAdded != null)
            {
                stockOnHoldToBeAdded.StockQty += stockQty;
            }
            else
            {
                stockOnHoldToBeAdded = new StockOnHold()
                {
                    SessionId = session.Id,
                    StockId = stockToSubtract.Id,
                    ExpireTime = DateTime.Now.AddMinutes(_basketOptions.Value.StockOnHoldMinutesExpired),
                    StockQty = stockQty,
                    ProductId = stockToSubtract.ProductId

                };

                _unitOfWork.StockOnHold.Add(stockOnHoldToBeAdded);

            }
            stockToSubtract.Quantity -= stockQty;

            return stockOnHoldToBeAdded;

        }


        private async Task ChangeExpireTimeForExistingStocksOnHold(StockTransferToOnHoldStockTypeEnum transferType, ISession session)
        {
            if (transferType == StockTransferToOnHoldStockTypeEnum.OneWithUpdatingExpireTimeForBasketProducts)
            {
                var stocksOnHoldToChangeExpireTime = await _unitOfWork.StockOnHold.FindAllAsync(s => s.SessionId == session.Id);

                if (stocksOnHoldToChangeExpireTime.Count() > 0)
                {
                    foreach (var stockOnHold in stocksOnHoldToChangeExpireTime)
                    {
                        stockOnHold.ExpireTime = DateTime.Now.AddMinutes(_basketOptions.Value.StockOnHoldMinutesExpired);
                    }
                }
            }
        }
    }
}
