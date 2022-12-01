using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Core.Dtos.PictureDtos
{
    public class PictureForCreationDto
    {
        public string Url { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        public PictureForCreationDto() 
        {
            DateAdded = DateTime.Now;
        }
    }
}
