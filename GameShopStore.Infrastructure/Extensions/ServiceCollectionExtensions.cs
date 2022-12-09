using GameShopStore.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Infrastructure.ApplicationDbContext>(x =>
            {
                //x.UseLazyLoadingProxies();
                x.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b =>
                {
                    b.MigrationsAssembly("GameShopStore.Infrastructure");
                });
            });
        }
    }
}
