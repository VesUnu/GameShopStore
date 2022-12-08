using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Interfaces
{
    public interface IJwtTokenHelper
    {
        Task<string> GenerateJwtToken(User user, UserManager<User> userManager, IConfiguration config);
    }
}
