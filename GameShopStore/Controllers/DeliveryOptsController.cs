using AutoMapper;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.DeliveryOptDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GameShopStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DeliveryOptsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeliveryOptsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DeliveryOptToReturnDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDeliveryOpts()
        {
            var deliveryOptions = await _unitOfWork.DeliveryOpt.GetAllAsync();

            if (!deliveryOptions.Any())
            {
                return NotFound();
            }

            var deliveryOptsToReturn = _mapper.Map<List<DeliveryOptToReturnDto>>(deliveryOptions);

            return Ok(deliveryOptsToReturn);
        }
    }
}
