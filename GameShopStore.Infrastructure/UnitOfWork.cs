using GameShopStore.Application.Interfaces;
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

        private IRequirementsRepo _requirements;
        public IRequirementsRepo Requirements
        {
            get
            {
                if (_requirements = null)
                {
                    _requirements = new RequirementsRepo(_appDbCtx)
                }

                return _requirements;
            }
        }
    }
}
