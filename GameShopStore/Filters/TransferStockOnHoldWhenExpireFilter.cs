using GameShopStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GameShopStore.Filters
{
    public class TransferStockOnHoldWhenExpireFilter : IAsyncActionFilter
    {
        private readonly ITransferStockOnHoldWhenExpired _transferStockOnHoldWhenExpire;
        private readonly ILogger _logger;

        public TransferStockOnHoldWhenExpireFilter(ITransferStockOnHoldWhenExpired transferStockOnHoldWhenExpired, ILogger<TransferStockOnHoldWhenExpireFilter> logger)
        {
            _transferStockOnHoldWhenExpire = transferStockOnHoldWhenExpired;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!await _transferStockOnHoldWhenExpire.Do())
            {
                _logger.LogWarning("Something went wrong during deleting Expired StockOnHold");
            }
            await next();
        }
    }

    public class TransferStockOnHoldWhenExpireAttribute : TypeFilterAttribute
    {
        public TransferStockOnHoldWhenExpireAttribute()
            : base(typeof(TransferStockOnHoldWhenExpireFilter))
        {
        }
    }
}
