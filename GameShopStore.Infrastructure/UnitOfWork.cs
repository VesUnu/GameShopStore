using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Models;
using GameShopStore.Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _appDbCtx;

        public UnitOfWork(ApplicationDbContext appDbCtx)
        {
            _appDbCtx = appDbCtx;
        }

        private IRequirementsRepo _requirements = null!;
        public IRequirementsRepo Requirements
        {
            get
            {
                if (_requirements == null)
                {
                    _requirements = new RequirementsRepo(_appDbCtx);
                }

                return _requirements;
            }
        }

        private ICategoryRepo _category = null!;
        public ICategoryRepo Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepo(_appDbCtx);
                }
                return _category;
            }
        }

        private ISubCategoryRepo _subCategory = null!;
        public ISubCategoryRepo SubCategory
        {
            get
            {
                if (_subCategory == null)
                {
                    _subCategory = new SubCategoryRepo(_appDbCtx);
                }
                return _subCategory;
            }
        }

        private ILanguageRepo _language = null!;
        public ILanguageRepo Language 
        {
            get
            {
                if (_language == null)
                {
                    _language = new LanguageRepo(_appDbCtx);
                }

                return _language;
            }
        }

        private IPictureRepo _picture = null!;
        public IPictureRepo Picture 
        {
            get
            {
                if (_picture == null)
                {
                    _picture = new PictureRepo(_appDbCtx);
                }
                return _picture;

            }
        }

        private IUserRepo _user = null!;
        public IUserRepo User 
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepo(_appDbCtx);
                }
                return _user;
            }
        }

        private IProductRepo _product = null!;
        public IProductRepo Product 
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepo(_appDbCtx);
                }
                return _product;
            }
        }

        private IStockRepo _stock = null!;
        public IStockRepo Stock 
        {
            get
            {
                if (_stock == null)
                {
                    _stock = new StockRepository(_appDbCtx);
                }
                return _stock;
            }
        }

        private IOrderRepo _order = null!;
        public IOrderRepo Order 
        {
            get
            {
                if (_order == null)
                {
                    _order = new OrderRepo(_appDbCtx);
                }
                return _order;
            }
        }

        private IStockOnHoldRepo _stockOnHold = null!;
        public IStockOnHoldRepo StockOnHold 
        {
            get
            {
                if (_stockOnHold == null)
                {
                    _stockOnHold = new StockOnHoldRepo(_appDbCtx);
                }
                return _stockOnHold;
            }
        }

        private IAddressRepo _address = null!;
        public IAddressRepo Address 
        {
            get
            {
                if (_address == null)
                {
                    _address = new AddressRepo(_appDbCtx);
                }
                return _address;
            }
        }

        private IDeliveryRepo _deliveryOpt = null!;
        public IDeliveryRepo DeliveryOpt 
        {
            get
            {
                if (_deliveryOpt == null)
                {
                    _deliveryOpt = new DeliveryOptionRepo(_appDbCtx);
                }
                return _deliveryOpt;
            }
        }
        public async Task<bool> SaveAsync()
        {
            return await _appDbCtx.SaveChangesAsync() > 0;
        }

        public bool IsAnyEntityModified()
        {
            return _appDbCtx.ChangeTracker.Entries().Any(x => x.State == EntityState.Modified);
        }

        public bool IsAnyEntityAdded()
        {
            return _appDbCtx.ChangeTracker.Entries().Any(x => x.State == EntityState.Added);
        }

        public bool IsAnyEntityDeleted()
        {
            return _appDbCtx.ChangeTracker.Entries().Any(x => x.State.Equals(EntityState.Deleted));
        }
    }
}
