using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class SubCategoryRepo : BaseRepo<SubCategory>, ISubCategoryRepo
    {
        public SubCategoryRepo(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {
        }
    }
}
