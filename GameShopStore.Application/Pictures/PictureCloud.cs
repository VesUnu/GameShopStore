using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameShopStore.Application.Helpers;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.PictureDtos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Pictures
{
    public class PictureCloud : IAddPictureToCloud
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryOpts;

        public PictureCloud(IOptions<CloudinarySettings> cloudinaryOpts)
        {
            _cloudinaryOpts = cloudinaryOpts;
        }

        public bool Do(Cloudinary cloudinary, PictureForCreationDto picCreateDto)
        {
            var file = picCreateDto.File;

            var result = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(_cloudinaryOpts.Value.ImageWidth).Height(_cloudinaryOpts.Value.ImageHeight)

                    };

                    result = cloudinary.Upload(uploadParams);
                }
            }

            if (result.Url == null)
            {
                return false;
            }

            picCreateDto.Url = result.Url?.ToString();
            picCreateDto.PublicId = result.PublicId;

            return true;
        }
    }
}
