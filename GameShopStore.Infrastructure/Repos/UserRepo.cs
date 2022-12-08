using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.UserDtos;
using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {
        }

        public async Task<IEnumerable<UserListDto>> GetAllWithRolesAsync()
        {
            var usersToReturn = await _appDbCtx.Users.OrderBy(x => x.UserName)
             .Select(user => new UserListDto
             {
                 Id = user.Id,
                 UserName = user.UserName,
                 Roles = (from userRole in user.UserRoles
                          join role in _appDbCtx.Roles
                          on userRole.RoleId
                          equals role.Id
                          select role.Name).ToList()
             }).ToListAsync();

            return usersToReturn;
        }

        public async Task<UserAccInfoDto> GetForAccInfoAsync(int id)
        {
            return await _appDbCtx.Users.Where(u => u.Id == id).Select(u => new UserAccInfoDto
            {
                Name = u.UserName,
                Surname = u.SurName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber
            }).FirstOrDefaultAsync();
        }
    }
}
