using CloudinaryDotNet;
using GameShopStore.Core.Dtos.PictureDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Interfaces
{
    public interface IAddPictureToCloud
    {
        bool Do(Cloudinary cloudinary, PictureForCreationDto pictureForCreationDto);
    }
}
