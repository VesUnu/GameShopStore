using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GameShopStore.Core.Models
{
    public class User : IdentityUser<int>
    {
        public string SurName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
