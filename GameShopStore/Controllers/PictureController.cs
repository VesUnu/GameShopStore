using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameShopStore.Application.Helpers;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.PictureDtos;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GameShopStore.Controllers
{
    [Route("api/admin/product/{productId}/photos")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IUnitOfWork _unitOfWork;
        private Cloudinary _cloudinary;
        private readonly IAddPictureToCloud _addPictureToCloud;

        public PictureController(IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig, IUnitOfWork unitOfWork, Cloudinary cloudinary, IAddPictureToCloud addPictureToCloud)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;

            Account acc = new Account(_cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);

            _addPictureToCloud = addPictureToCloud;
        }

        [HttpGet("id", Name = "GetPicture")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _unitOfWork.Picture.GetAsync(id);

            if (photoFromRepo == null)
            {
                return NotFound();
            }

            var photo = _mapper.Map<PictureForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        [Authorize(Policy = "ModerateProductRole")]
        public async Task<IActionResult> AddPictureForProduct(int productId,
            [FromForm] PictureForCreationDto pictureForCreationDto)
        {
            var productFromRepo = await _unitOfWork.Product.GetWithPicturesOnly(productId);


            var file = pictureForCreationDto.File;

            if (file == null)
            {
                return BadRequest("The picture file wasn't sent");
            }

            if (!_addPictureToCloud.Do(_cloudinary, pictureForCreationDto))
            {
                return BadRequest("The file wasn't saved in cloud");
            }

            var picture = _mapper.Map<Picture>(pictureForCreationDto);

            if (!productFromRepo.Pictures.Any(p => p.isMain))
            {
                picture.isMain = true;
            }

            productFromRepo.Pictures.Add(picture);



            if (await _unitOfWork.SaveAsync())
            {
                var photoToReturn = _mapper.Map<PictureForReturnDto>(picture);
                return CreatedAtRoute("GetPicture", new { productId = productId, id = picture.Id }, photoToReturn);
            }

            return BadRequest($"Could not add picture for product: {productId}");

        }


        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPicture(int productId, int id)
        {
            var product = await _unitOfWork.Product.GetWithPicturesOnly(productId);

            if (!product.Pictures.Any(p => p.Id == id))
            {
                return BadRequest("Trying to change picture that do not exists for that product");
            }

            var pictureFromRepo = await _unitOfWork.Picture.GetAsync(id);

            if (pictureFromRepo.isMain)
            {
                return BadRequest("This is your main picture");
            }

            var currrentMainPicture = await _unitOfWork.Picture.FindAsync(p => p.ProductId == productId && p.isMain == true);

            if (currrentMainPicture != null)
            {
                currrentMainPicture.isMain = false;
            }


            pictureFromRepo.isMain = true;

            if (await _unitOfWork.SaveAsync())
            {
                return NoContent();
            }

            return BadRequest("The main photo could not be saved");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(int productId, int id)
        {
            var product = await _unitOfWork.Product.GetWithPicturesOnly(productId);

            if (!product.Pictures.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var pictureFromRepo = await _unitOfWork.Picture.GetAsync(id);

            if (pictureFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(pictureFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _unitOfWork.Picture.Delete(pictureFromRepo);
                }
            }
            else
            {
                _unitOfWork.Picture.Delete(pictureFromRepo);
            }


            if (await _unitOfWork.SaveAsync())
            {
                return NoContent();
            }

            return BadRequest("Deleting the picture process has failed");
        }
    }
}
