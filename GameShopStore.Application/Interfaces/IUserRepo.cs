using GameShopStore.Core.Dtos.UserDtos;
using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IUserRepo : IBaseRepository<User>
    {
        Task<IEnumerable<UserListDto>> GetAllWithRolesAsync();
        Task<UserAccInfoDto> GetForAccInfoAsync(int id);
    }
}
