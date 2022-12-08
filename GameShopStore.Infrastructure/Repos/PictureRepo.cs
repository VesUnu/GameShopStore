using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class PictureRepo : BaseRepo<Picture>, IPictureRepo
    {
        public PictureRepo(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {

        }

        public async Task<IEnumerable<Picture>> GetPicturesForProduct(int productId)
        {
            var picture = await _appDbCtx.Pictures.Where(p => p.ProductId == productId).Select(p => new Picture
            {
                Id = p.Id,
                Url = p.Url,
                DateAdded = p.DateAdded,
                isMain = p.isMain,
                ProductId = p.ProductId
            }).ToListAsync();

            return picture;
        }
    }
}
