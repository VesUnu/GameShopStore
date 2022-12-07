using GameShopStore.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.StockManipulations
{
    public class TransferExpiredStockOnHold : ITransferStockOnHoldWhenExpired
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransferExpiredStockOnHold(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Do() 
        {
            var stockOnHoldTransfer = await _unitOfWork.StockOnHold.FindAllAsync(x => x.ExpireTime < DateTime.Now);

            bool result = true;

            if (stockOnHoldTransfer.Any())
            {
                try
                {
                    foreach (var stockOnHold in stockOnHoldTransfer)
                    {
                        var stockToUpdate = await _unitOfWork.Stock.GetAsync(stockOnHold.StockId);
                        if (stockToUpdate == null)
                        {
                            throw new NullReferenceException("StockId from StockOnHold not exist in Stock Entity - that's unexpected");
                        }
                        stockToUpdate.Quantity += stockOnHold.StockQty;
                    }

                    _unitOfWork.StockOnHold.DeleteRange(stockOnHoldTransfer);

                    result = await _unitOfWork.SaveAsync() ? true : false;

                    return result;
                }
                catch (DbUpdateConcurrencyException)
                {

                    return false;
                }
            }

            return result;
        }
    }
}
