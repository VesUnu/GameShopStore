using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Models;
using GameShopStore.Core.Dtos.AddressDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class AddressRepo : BaseRepo<Address>, IAddressRepo
    {
        public AddressRepo(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {

        }
    }
}
