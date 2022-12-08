using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.RequirementsDtos;
using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public class RequirementsRepo : BaseRepo<Requirements>, IRequirementsRepo
    {
        public RequirementsRepo(ApplicationDbContext appDbCtx) : base(appDbCtx)
        {
            
        }

        public async Task<EditingRequirementsDto> GetRequirementsForProductAsync(int id)
        {
            var result = await _appDbCtx.Requirements.Where(r => r.ProductId == id)
                        .Select(requriements => new EditingRequirementsDto
                        {
                            OS = requriements.OS,
                            Processor = requriements.Processor,
                            RAM = requriements.RAM,
                            GraphicsCard = requriements.GraphicsCard,
                            HDD = requriements.HDD,
                            IsNetworkConnectionRequire = requriements.IsNetworkConnectionRequire
                        }).FirstOrDefaultAsync();

            return result;
        }
    }
}
