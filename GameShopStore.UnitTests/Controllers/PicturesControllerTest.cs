using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using FluentAssertions;
using GameShopStore.Controllers;
using GameShopStore.Core.Dtos.PictureDtos;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GameShopStore.UnitTests.Controllers
{
    public class PicturesControllerTest : UnitTestBase, IDisposable
    {
        private readonly PictureController _cut;

        public PicturesControllerTest()
        {
            _cut = new PictureController(_mapper, _cloudinaryConfig, _mockedUnitOfWork.Object, _mockedAddPictureToCloud.Object);
        }

        [Fact]
        public void Given_PictureId_When_GetPicture_ThenReturn_OkStatusWithPictureForReturnDto()
        {
            //Arrange
            var photoId = 1;

            _mockedUnitOfWork.Setup(s => s.Picture.GetAsync(photoId)).ReturnsAsync(new Picture());

            //Act
            var result = _cut.GetPhoto(photoId).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull();
            result.As<OkObjectResult>().Value.Should().BeOfType<PictureForReturnDto>();

            _mockedUnitOfWork.Verify(v => v.Picture.GetAsync(photoId), Times.Once);
        }

        [Fact]
        public void Given_PhotoId_When_GetPhoto_ThenReturn_NotFound()
        {
            //Arrange
            var photoId = 1;

            _mockedUnitOfWork.Setup(s => s.Picture.GetAsync(photoId)).ReturnsAsync((Picture)null);

            //Act
            var result = _cut.GetPhoto(photoId).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.Picture.GetAsync(photoId), Times.Once);
        }


        [Fact]
        public void Given_ProductIdAndPictureForCreationWithoutImage_When_AddPictureForProduct_ThenReturn_BadRequest_BecausePhotoFileWasntSent()
        {
            //Arrange
            int productId = 1;
            var pictureCreationDto = new PictureForCreationDto();

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                    .ReturnsAsync(new Product());


            //Act
            var result = _cut.AddPictureForProduct(productId, pictureCreationDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Photo file wasn't sent");

            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndPictureForCreation_When_AddPictureForProductAndThenReturnWithBadRequest_BecauseErrorOccuredDuringPictureSaving()
        {
            //Arrange

            int productId = 1;
            var pictureCreationDto = new PictureForCreationDto() { File = _mockedFormFile.Object };


            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                    .ReturnsAsync(new Product() { Pictures = new List<Picture>() });

            _mockedAddPictureToCloud.Setup(s => s.Do(It.IsAny<Cloudinary>(), pictureCreationDto)).Returns(true);

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(false);


            //Act
            var result = _cut.AddPictureForProduct(productId, pictureCreationDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be($"Could not add photo for product: {productId}");

            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndPictureForCreation_When_AddPictureForProductInCaseWhenProductDoNotHaveMainPictureAndThenVerifyIfPhotoWasSetAsMain()
        {
            //Arrange

            int productId = 1;
            var pictureCreationDto = new PictureForCreationDto() { File = _mockedFormFile.Object };
            var emptyPhotoList = new List<Picture>();

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                    .ReturnsAsync(new Product() { Pictures = emptyPhotoList });

            _mockedAddPictureToCloud.Setup(s => s.Do(It.IsAny<Cloudinary>(), pictureCreationDto)).Returns(true);

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);


            //Act
            var result = _cut.AddPictureForProduct(productId, pictureCreationDto).Result;

            //Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
            result.As<CreatedAtRouteResult>().Value.As<PictureForReturnDto>().isMain.Should().BeTrue();

            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndPictureForCreation_When_AddPictureForProductInCaseWhenProductAlreadyHaveMainPictureAndThenVerifyIfPhotoWasntSetAsMain()
        {
            //Arrange

            int productId = 1;
            var pictureCreationDto = new PictureForCreationDto() { File = _mockedFormFile.Object };
            var productPhotoList = new List<Picture>() { new Picture() { isMain = true } };

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                    .ReturnsAsync(new Product() { Pictures = productPhotoList });

            _mockedAddPictureToCloud.Setup(s => s.Do(It.IsAny<Cloudinary>(), pictureCreationDto)).Returns(true);

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);


            //Act
            var result = _cut.AddPictureForProduct(productId, pictureCreationDto).Result;

            //Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
            result.As<CreatedAtRouteResult>().Value.As<PictureForReturnDto>().isMain.Should().BeFalse();

            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndPictureForCreation_When_AddPictureForProductAndThenReturn_CreatedStatusWithRouteToPictureForReturnDtoValue()
        {
            //Arrange

            int productId = 1;
            int exptectedPhotoId = 0;
            var photoForCreationDto = new PictureForCreationDto() { File = _mockedFormFile.Object };
            var productPhotoList = new List<Picture>() { new Picture() { isMain = true } };

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                    .ReturnsAsync(new Product() { Pictures = productPhotoList });

            _mockedAddPictureToCloud.Setup(s => s.Do(It.IsAny<Cloudinary>(), photoForCreationDto)).Returns(true);

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);


            //Act
            var result = _cut.AddPictureForProduct(productId, photoForCreationDto).Result;

            //Assert
            result.Should().BeOfType<CreatedAtRouteResult>();

            result.As<CreatedAtRouteResult>().Value.Should().BeOfType<PictureForReturnDto>();

            result.As<CreatedAtRouteResult>().Value.Should().NotBeNull();

            result.As<CreatedAtRouteResult>().RouteValues.Values.First().Should().Be(productId);
            result.As<CreatedAtRouteResult>().RouteValues.Values.Skip(1).First().Should().Be(exptectedPhotoId);

            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndPhotoId_When_SetMainPictureInCaseWhenPhotoIsNotMainAndThenReturnWithNoContent()
        {
            //Arrange
            int productId = 1;
            int photoId = 2;

            int photoIdOfCurrentMainPhoto = 1;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                                .ReturnsAsync(new Product()
                                {
                                    Pictures = new List<Picture>()
                                {
                                    new Picture(){Id = photoIdOfCurrentMainPhoto, isMain = true},
                                    new Picture(){Id = photoId}
                                }
                                });

            _mockedUnitOfWork.Setup(s => s.Picture.GetAsync(photoId))
                                .ReturnsAsync(new Picture() { Id = photoId, isMain = false });

            _mockedUnitOfWork.Setup(s => s.Picture.FindAsync(p => p.ProductId == productId && p.isMain == true))
                                .ReturnsAsync(new Picture() { Id = photoIdOfCurrentMainPhoto, isMain = true });

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);


            //Act
            var result = _cut.SetMainPicture(productId, photoId).Result;

            //Assert
            result.Should().BeOfType<NoContentResult>();

            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Picture.GetAsync(photoId), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Picture.FindAsync(p => p.ProductId == productId && p.isMain == true), Times.Once);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);


        }

        [Fact]
        public void Given_ProductIdAndPictureIdThatProductDoNotHave_When_SetMainPictureAndThenReturnWithBadRequestWithMessage()
        {
            //Arrange
            int productId = 1;
            int photoIdThatProductDoNotHave = 2;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                                .ReturnsAsync(new Product()
                                {
                                    Pictures = new List<Picture>()
                                {
                                    new Picture(){Id = 1}
                                }
                                });



            //Act
            var result = _cut.SetMainPicture(productId, photoIdThatProductDoNotHave).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Trying to change photo that do not exists for that product");


            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndPictureIdThatIsAlreadyMainPicture_When_SetMainPictureAndThenReturnWithBadRequestWithMessage()
        {
            //Arrange
            int productId = 1;
            int photoId = 2;

            var pictureFromProductThatIsAlreadyMainPhoto = new Picture() { Id = photoId, isMain = true };

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                                .ReturnsAsync(new Product()
                                {
                                    Pictures = new List<Picture>()
                                {
                                    pictureFromProductThatIsAlreadyMainPhoto
                                }
                                });

            _mockedUnitOfWork.Setup(s => s.Picture.GetAsync(photoId))
                        .ReturnsAsync(pictureFromProductThatIsAlreadyMainPhoto);


            //Act
            var result = _cut.SetMainPicture(productId, photoId).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("This is already main photo");


            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.Picture.GetAsync(photoId), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndictureId_When_SetMainPicture_ThenReturn_BadRequestWithMessage_BecauseErrorOccuredDuringSaving()
        {
            //Arrange
            int productId = 1;
            int photoId = 2;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                                .ReturnsAsync(new Product()
                                {
                                    Pictures = new List<Picture>()
                                {
                                    new Picture(){Id = photoId}
                                }
                                });

            _mockedUnitOfWork.Setup(s => s.Picture.GetAsync(photoId))
                        .ReturnsAsync(new Picture());

            _mockedUnitOfWork.Setup(s => s.Picture.FindAsync(p => p.ProductId == productId && p.isMain == true))
                        .ReturnsAsync(new Picture());

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(false);


            //Act
            var result = _cut.SetMainPicture(productId, photoId).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Could not save photo as main photo");


            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.Picture.GetAsync(photoId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.Picture.FindAsync(p => p.ProductId == productId && p.isMain == true), Times.Once);
            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);

        }


        [Fact]
        public void Given_ProductIdAndPictureId_When_DeletePictureAndThenReturn_NoContent()
        {
            //Arrange
            int productId = 1;
            int photoId = 2;

            var photoFromProductThatIsSameAsPassedOne = new Picture() { Id = 2 };

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                                .ReturnsAsync(new Product()
                                {
                                    Pictures = new List<Picture>()
                                {
                                    photoFromProductThatIsSameAsPassedOne
                                }
                                });

            _mockedUnitOfWork.Setup(s => s.Picture.GetAsync(photoId)).ReturnsAsync(new Picture() { Id = photoId });

            _mockedUnitOfWork.Setup(s => s.Picture.Delete(It.IsAny<Picture>()));

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);

            //Act
            var result = _cut.DeletePicture(productId, photoId).Result;

            //Assert
            result.Should().BeOfType<NoContentResult>();

            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Picture.GetAsync(photoId), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Picture.Delete(It.IsAny<Picture>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);
        }

        [Fact]
        public void Given_ProductIdAndPictureIdThatNotExistForThatProduct_When_DeletePictureAndThenReturn_Unauthorized()
        {
            //Arrange
            int productId = 1;
            int photoId = 2;
            int differentPictureIdThenThePassedOne = 3;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                                .ReturnsAsync(new Product()
                                {
                                    Pictures = new List<Picture>()
                                {
                                    new Picture(){Id = differentPictureIdThenThePassedOne}
                                }
                                });

            //Act
            var result = _cut.DeletePicture(productId, photoId).Result;

            //Assert
            result.Should().BeOfType<UnauthorizedResult>();

            _mockedUnitOfWork.Verify(v => v.Product.GetWithPicturesOnly(productId), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndPictureId_When_DeletePicture_CaseWhenPictureForDeleteIsFromCloud_VerifyIfPictureDeleteMethodWasntCalled_BecauseResultFromCloudinaryIsNotOk()
        {
            //Arrange
            int productId = 1;
            int photoId = 2;

            var photoFromProductThatIsSameAsPassedOne = new Picture() { Id = photoId, PublicId = "Placeholder PublicId" };

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                                .ReturnsAsync(new Product()
                                {
                                    Pictures = new List<Picture>()
                                {
                                    photoFromProductThatIsSameAsPassedOne
                                }
                                });

            _mockedUnitOfWork.Setup(s => s.Picture.GetAsync(photoId)).ReturnsAsync(new Picture() { Id = photoId, PublicId = "Placeholder PublicId" });

            _mockedUnitOfWork.Setup(s => s.Picture.Delete(It.IsAny<Picture>()));

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(false);


            //Act
            var result = _cut.DeletePicture(productId, photoId).Result;

            //Assert

            _mockedUnitOfWork.Verify(v => v.Picture.Delete(It.IsAny<Picture>()), Times.Never);
        }

        [Fact]
        public void Given_ProductIdAndPictureId_When_DeletePicture_CaseWhenPictureForDeleteIsFromCloud_ThenRetun_BadRequestWithMessage_BecauseErrorOccuredDuringSaving()
        {
            //Arrange
            int productId = 1;
            int photoId = 2;

            var photoFromProductThatIsSameAsPassedOne = new Picture() { Id = photoId, PublicId = "Placeholder PublicId" };

            _mockedUnitOfWork.Setup(s => s.Product.GetWithPicturesOnly(productId))
                                .ReturnsAsync(new Product()
                                {
                                    Pictures = new List<Picture>()
                                {
                                    photoFromProductThatIsSameAsPassedOne
                                }
                                });

            _mockedUnitOfWork.Setup(s => s.Picture.GetAsync(photoId)).ReturnsAsync(new Picture() { Id = photoId, PublicId = "Placeholder PublicId" });

            _mockedUnitOfWork.Setup(s => s.Picture.Delete(It.IsAny<Picture>()));

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(false);


            //Act
            var result = _cut.DeletePicture(productId, photoId).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Failed to delete the photo");

            _mockedUnitOfWork.Verify(v => v.Picture.Delete(It.IsAny<Picture>()), Times.Never);
        }
    }
}
