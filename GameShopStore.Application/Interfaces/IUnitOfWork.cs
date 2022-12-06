using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepo Category { get; }
        ISubCategoryRepo SubCategory { get; }
        ILanguageRepo Language { get; }
        IPictureRepo Picture { get; }
        IUserRepo User { get; }
        IProductRepo Product { get; }
        IRequirementsRepo Requirements { get; }
        IStockRepo Stock { get; }
        IOrderRepo Order { get; }
        IStockOnHoldRepo StockOnHold { get; }
        IAddressRepo Address { get; }
        IDeliveryRepo DeliveryOpt { get; }
        Task<bool> SaveAsync();
        bool IsAnyEntityModified();
        bool IsAnyEntityAdded();
        bool IsAnyEntityDeleted();
    }
}
