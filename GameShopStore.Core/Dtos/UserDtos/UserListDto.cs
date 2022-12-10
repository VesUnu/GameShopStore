using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.UserDtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public ICollection<string> Roles { get; set; } = null!;
    }
}
