using AutoMapper;
using GameShopStore.Core.Dtos.AddressDtos;
using GameShopStore.Core.Dtos.CategoryDtos;
using GameShopStore.Core.Dtos.DeliveryOptDto;
using GameShopStore.Core.Dtos.LanguageDtos;
using GameShopStore.Core.Dtos.PictureDtos;
using GameShopStore.Core.Dtos.RequirementsDtos;
using GameShopStore.Core.Dtos.SubCategoryDtos;
using GameShopStore.Core.Dtos.UserDtos;
using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Application.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreationRequirementsDto, Requirements>();
            CreateMap<EditingRequirementsDto, Requirements>();
            CreateMap<Category, CategoryToReturnDto>();
            CreateMap<SubCategory, SubCategoryReturnDto>();
            CreateMap<Language, ReturnLanguagesDto>();
            CreateMap<Picture, PictureForCreationDto>();
            CreateMap<Picture, PictureForReturnDto>();
            CreateMap<UserAccInfoEditDto, User>()
                .ForMember(d => d.UserName, x => x.MapFrom(s => s.Name));
            CreateMap<Product, Product>()
                .ForMember(d => d.Pictures, o => o.Ignore())
                .ForMember(d => d.Stock, o => o.Ignore());
            CreateMap<UserAddressInfoForCreationDto, Address>();
            CreateMap<UserAddressInfoDtoForEdit, Address>();
            CreateMap<Address, UserAddressListDto>();
            CreateMap<DeliveryOpt, DeliveryOptToReturnDto>();
        }
    }
}
