using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using GameShopStore.Application.Helpers;
using Microsoft.Extensions.Configuration;
using GameShopStore.Core.Models;

namespace GameShopStore.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly IConfiguration _seedDataOptions;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration seedDataOptions)
            : base(options)
        {
            _seedDataOptions = seedDataOptions;
        }
    }
}