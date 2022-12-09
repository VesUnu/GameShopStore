using AutoMapper;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.AddressDtos;
using GameShopStore.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}/user/{userId}", Name = "GetAddress")]
        [AdminOrUserWithSameUserIdFilter("userId")]
        public async Task<IActionResult> GetAddress(int id, int userId)
        {
            var address = await _unitOfWork.Address.FindAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpGet("user/{userId}")]
        [AdminOrUserWithSameUserIdFilter("userId")]
        public async Task<IActionResult> GetAddressesForUser(int userId)
        {
            var addresses = await _unitOfWork.Address.FindAllAsync(x => x.UserId == userId);

            var addressesToReturn = _mapper.Map<IEnumerable<UserAddressListDto>>(addresses);

            if (!addresses.Any())
            {
                return NotFound();
            }

            return Ok(addressesToReturn);
        }

        [HttpDelete("delete/{id}/user/{userId}")]
        [AdminOrUserWithSameUserIdFilter("userId")]
        public async Task<IActionResult> DeleteAddress(int id, int userId)
        {
            var address = await _unitOfWork.Address.FindAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound();
            }

            _unitOfWork.Address.Delete(address);

            if (!await _unitOfWork.SaveAsync())
            {
                return BadRequest("Something went wrong during the process of deleting the address");
            }

            return NoContent();
        }
    }
}
