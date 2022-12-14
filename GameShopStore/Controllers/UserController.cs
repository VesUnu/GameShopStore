using AutoMapper;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.AddressDtos;
using GameShopStore.Core.Dtos.UserDtos;
using GameShopStore.Core.Models;
using GameShopStore.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _usermanager;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _usermanager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.User.GetAllOrderedByAsync(u => u.Id);

            if (users.Count() < 1)
            {
                return NotFound();
            }


            return Ok(users);
        }

        [HttpGet("{id}")]
        [AdminOrUserWithSameUserIdFilter]
        public async Task<IActionResult> GetUserForAccInfo(int id)
        {
            var user = await _unitOfWork.User.GetForAccInfoAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("{id}/edit-acc")]
        [AdminOrUserWithSameUserIdFilter]
        public async Task<IActionResult> EditUserAccInfo(int id, UserAccInfoEditDto userAccInfoToEditDto)
        {
            var userToEdit = await _usermanager.FindByIdAsync(id.ToString());

            if (userToEdit == null)
            {
                return NotFound();
            }

            userToEdit = _mapper.Map(userAccInfoToEditDto, userToEdit);

            var result = await _usermanager.UpdateAsync(userToEdit);

            if (!result.Succeeded)
            {
                return BadRequest("Oops, something went wrong during editing the User account information");
            }


            return Ok(userToEdit);
        }


        [HttpPost("{id}/create-address")]
        [AdminOrUserWithSameUserIdFilter]
        public async Task<IActionResult> CreateUserAddress(int id,
                UserAddressInfoForCreationDto userAddressInfoForCreationDto)
        {
            var result = _mapper.Map<Address>(userAddressInfoForCreationDto);
            result.UserId = id;
            _unitOfWork.Address.Add(result);

            if (!await _unitOfWork.SaveAsync())
            {
                return BadRequest("Oops, something went wrong during creating Address for User: " + id);
            }
            return CreatedAtRoute("GetAddress", new { id = result.Id, userId = result.Id }, result);
        }

        [HttpPut("{id}/edit-address/{addressId}")]
        [AdminOrUserWithSameUserIdFilter]
        public async Task<IActionResult> EditUserAddress(int id, int addressId,
            UserAddressInfoDtoForEdit userAddressInfoForEditDto)
        {
            var address = await _unitOfWork.Address.FindAsync(a => a.Id == addressId && a.UserId == id);

            if (address == null)
            {
                return NotFound();
            }

            _mapper.Map(userAddressInfoForEditDto, address);


            if (!await _unitOfWork.SaveAsync())
            {
                return BadRequest("Oops, something went wrong during editing Address for User: " + id + "or passed same value to edit as Address already have");
            }

            return Ok(address);
        }

    }
}
