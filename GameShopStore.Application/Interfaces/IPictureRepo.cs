using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IPictureRepo : IBaseRepository<Picture>
    {
        Task<IEnumerable<Picture>> GetPicturesForProduct(int productId);
    }
}
