using GameShopStore.Controllers;
using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using GameShopStore.Core.Dtos.UserDtos;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Identity;
using GameShopStore.Application.Helpers;
using Newtonsoft.Json;
using GameShopStore.Core.Dtos.ProductDtos;
using GameShopStore.Core.Dtos.RequirementsDtos;
using System.Linq.Expressions;
using GameShopStore.Core.Dtos.SubCategoryDtos;
using GameShopStore.Core.Dtos.CategoryDtos;
using GameShopStore.Core.Dtos.LanguageDtos;
using GameShopStore.UnitTests.DataTests;

namespace GameShopStore.UnitTests.Controllers
{
    public class AdminControllerTest : UnitTestBase, IDisposable
    {
        private readonly AdminController _cut;
        public AdminControllerTest()
        {
            var controllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            _cut = new AdminController(_mockedUserManager.Object, _mapper,
                            _cloudinaryConfig, _mockedUnitOfWork.Object)
            {
                ControllerContext = controllerContext
            };
        }


        [Fact]
        public void Given_None_When_GetUsersWithRoles_ThenReturn_OkStatusWithUsersList()
        {
            //Arrange
            _mockedUnitOfWork.Setup(s => s.User.GetAllWithRolesAsync())
                        .Returns(Task.FromResult<IEnumerable<UserListDto>>(new List<UserListDto>()));

            //Act
            var result = _cut.GetUsersWithRoles().Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull();

            _mockedUnitOfWork.Verify(v => v.User.GetAllWithRolesAsync(), Times.Once);
        }

        [Fact]
        public void Given_None_When_GetUsersWithRoles_ThenReturn_NotFound_BecauseUsersNotExistInDb()
        {
            //Arrange
            _mockedUnitOfWork.Setup(s => s.User.GetAllWithRolesAsync())
                        .Returns(Task.FromResult<IEnumerable<UserListDto>>(null));

            //Act
            var result = _cut.GetUsersWithRoles().Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.User.GetAllWithRolesAsync(), Times.Once);
        }


        [Fact]
        public void Given_UserNameThatDoNotHaveAnyRole_And_RoleEditDtoForUser_When_EditRoles_ThenReturn_OkStatusWithUserRoles()
        {
            //Arrange
            var userName = "TestUser";
            var roleEditDto = new EditingRoleDto()
            {
                RoleNames = new string[] { "Customer", "Moderator" }
            };

            var emptyRoleList = new List<string>();

            var expectedRoles = new List<string>();
            expectedRoles.AddRange(roleEditDto.RoleNames);

            var listOfRolesToAdd = roleEditDto.RoleNames.Except(emptyRoleList);

            _mockedUserManager.Setup(s => s.FindByNameAsync(userName))
                        .Returns(Task.FromResult(new User() { UserName = userName }));

            _mockedUserManager.SetupSequence(s => s.GetRolesAsync(It.IsAny<User>()))
                        .Returns(Task.FromResult<IList<string>>(emptyRoleList))
                        .Returns(Task.FromResult<IList<string>>(expectedRoles));

            _mockedUserManager.Setup(s => s.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd))
                        .Returns(Task.FromResult(IdentityResult.Success));

            _mockedUserManager.Setup(s => s.RemoveFromRolesAsync(It.IsAny<User>(), emptyRoleList))
                        .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = _cut.EditRoles(userName, roleEditDto).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedRoles);

            _mockedUserManager.Verify(v => v.FindByNameAsync(userName), Times.Once);

            _mockedUserManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Exactly(2));

            _mockedUserManager.Verify(v => v.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd), Times.Once);

            _mockedUserManager.Verify(v => v.RemoveFromRolesAsync(It.IsAny<User>(), emptyRoleList), Times.Once);
        }

        [Fact]
        public void Given_UserNameAndEmptyRoleEditDto_When_EditRoles_ThenReturn_OkStatusWithEmptyUserRoles()
        {
            //Arrange
            var userName = "TestUser";
            var emptyRoleEditDto = new EditingRoleDto()
            {
                RoleNames = new string[0]
            };

            var rolesThatUserAlreadyHave = new string[] { "Customer", "Moderator" };

            var expectedEmptyRoles = new List<string>();
            expectedEmptyRoles.AddRange(emptyRoleEditDto.RoleNames);

            var listOfRolesToAdd = emptyRoleEditDto.RoleNames.Except(rolesThatUserAlreadyHave);

            _mockedUserManager.Setup(s => s.FindByNameAsync(userName))
                        .Returns(Task.FromResult(new User() { UserName = userName }));

            _mockedUserManager.SetupSequence(s => s.GetRolesAsync(It.IsAny<User>()))
                        .Returns(Task.FromResult<IList<string>>(rolesThatUserAlreadyHave))
                        .Returns(Task.FromResult<IList<string>>(expectedEmptyRoles));

            _mockedUserManager.Setup(s => s.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd))
                        .Returns(Task.FromResult(IdentityResult.Success));

            _mockedUserManager.Setup(s => s.RemoveFromRolesAsync(It.IsAny<User>(), rolesThatUserAlreadyHave))
                        .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = _cut.EditRoles(userName, emptyRoleEditDto).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.As<List<string>>().Should().BeEmpty();

            _mockedUserManager.Verify(v => v.FindByNameAsync(userName), Times.Once);

            _mockedUserManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Exactly(2));

            _mockedUserManager.Verify(v => v.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd), Times.Once);

            _mockedUserManager.Verify(v => v.RemoveFromRolesAsync(It.IsAny<User>(), rolesThatUserAlreadyHave), Times.Once);
        }

        [Fact]
        public void Given_UserNameAndRoleEditDtoThatHaveOneMoreRoleThenUserAlreadyHave_When_EditRoles_ThenReturn_OkStatusWithUserRoleWithOneMoreRole()
        {
            //Arrange
            var userName = "TestUser";
            var roleEditDto = new EditingRoleDto()
            {
                RoleNames = new string[] { "Customer", "Moderator", "Admin" }
            };

            var rolesThatUserAlreadyHave = new string[] { "Customer", "Moderator" };

            var expectedRoles = new List<string>();
            expectedRoles.AddRange(roleEditDto.RoleNames);

            var listOfRolesToAdd = roleEditDto.RoleNames.Except(rolesThatUserAlreadyHave);

            var listOFRoleToRemove = rolesThatUserAlreadyHave.Except(roleEditDto.RoleNames);

            _mockedUserManager.Setup(s => s.FindByNameAsync(userName))
                        .Returns(Task.FromResult(new User() { UserName = userName }));

            _mockedUserManager.SetupSequence(s => s.GetRolesAsync(It.IsAny<User>()))
                        .Returns(Task.FromResult<IList<string>>(rolesThatUserAlreadyHave))
                        .Returns(Task.FromResult<IList<string>>(expectedRoles));

            _mockedUserManager.Setup(s => s.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd))
                        .Returns(Task.FromResult(IdentityResult.Success));

            _mockedUserManager.Setup(s => s.RemoveFromRolesAsync(It.IsAny<User>(), listOFRoleToRemove))
                        .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = _cut.EditRoles(userName, roleEditDto).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.As<List<string>>().Should().BeEquivalentTo(expectedRoles);

            _mockedUserManager.Verify(v => v.FindByNameAsync(userName), Times.Once);

            _mockedUserManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Exactly(2));

            _mockedUserManager.Verify(v => v.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd), Times.Once);

            _mockedUserManager.Verify(v => v.RemoveFromRolesAsync(It.IsAny<User>(), listOFRoleToRemove), Times.Once);
        }

        [Fact]
        public void Given_UserNameThatAlreadyHaveRolesThatWasSelectedByClientAndRoleEditDtoForUser_When_EditRoles_ThenReturn_OkStatusWithUsersRoles_UserRolesDidntChangedBecauseClientPassedSameRolesAsUserAlreadyHave()
        {
            //Arrange
            var userName = "TestUser";
            var roleEditDto = new EditingRoleDto()
            {
                RoleNames = new string[] { "Customer", "Moderator" }
            };
            var emptyRoleList = new List<string>();

            var roleListThatUserAlreadyHave = new List<string>();
            roleListThatUserAlreadyHave.AddRange(new string[] { "Customer", "Moderator" });

            var expectedRoles = new List<string>();
            expectedRoles.AddRange(roleEditDto.RoleNames);

            var listOfRolesToAdd = roleEditDto.RoleNames.Except(roleListThatUserAlreadyHave);

            _mockedUserManager.Setup(s => s.FindByNameAsync(userName))
                        .Returns(Task.FromResult(new User() { UserName = userName }));

            _mockedUserManager.SetupSequence(s => s.GetRolesAsync(It.IsAny<User>()))
                        .Returns(Task.FromResult<IList<string>>(roleListThatUserAlreadyHave))
                        .Returns(Task.FromResult<IList<string>>(expectedRoles));

            _mockedUserManager.Setup(s => s.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd))
                        .Returns(Task.FromResult(IdentityResult.Success));

            _mockedUserManager.Setup(s => s.RemoveFromRolesAsync(It.IsAny<User>(), emptyRoleList))
                        .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = _cut.EditRoles(userName, roleEditDto).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedRoles);

            _mockedUserManager.Verify(v => v.FindByNameAsync(userName), Times.Once);

            _mockedUserManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Exactly(2));

            _mockedUserManager.Verify(v => v.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd), Times.Once);

            _mockedUserManager.Verify(v => v.RemoveFromRolesAsync(It.IsAny<User>(), emptyRoleList), Times.Once);
        }

        [Fact]
        public void Given_UserNameAndRoleEditDtoForUserWithRoleThatUserDoNotHave_When_EditRoles_ThenReturn_OkStatusWithUsersRoles_Added1MoreRoleForUserAndKeptRolesThatUserAlreadyHave()
        {
            //Arrange
            var userName = "TestUser";
            var roleEditDto = new EditingRoleDto()
            {
                RoleNames = new string[] { "Customer", "Moderator", "Admin" }
            };
            var emptyRoleList = new List<string>();

            var roleListThatUserAlreadyHave = new List<string>();
            roleListThatUserAlreadyHave.AddRange(new string[] { "Customer", "Moderator" });

            var expectedRoles = new List<string>();
            expectedRoles.AddRange(roleEditDto.RoleNames);

            var listOfRolesToAdd = roleEditDto.RoleNames.Except(roleListThatUserAlreadyHave);

            _mockedUserManager.Setup(s => s.FindByNameAsync(userName))
                        .Returns(Task.FromResult(new User() { UserName = userName }));

            _mockedUserManager.SetupSequence(s => s.GetRolesAsync(It.IsAny<User>()))
                        .Returns(Task.FromResult<IList<string>>(roleListThatUserAlreadyHave))
                        .Returns(Task.FromResult<IList<string>>(expectedRoles));

            _mockedUserManager.Setup(s => s.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd))
                        .Returns(Task.FromResult(IdentityResult.Success));

            _mockedUserManager.Setup(s => s.RemoveFromRolesAsync(It.IsAny<User>(), emptyRoleList))
                        .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = _cut.EditRoles(userName, roleEditDto).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedRoles);

            _mockedUserManager.Verify(v => v.FindByNameAsync(userName), Times.Once);

            _mockedUserManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Exactly(2));

            _mockedUserManager.Verify(v => v.AddToRolesAsync(It.IsAny<User>(), listOfRolesToAdd), Times.Once);

            _mockedUserManager.Verify(v => v.RemoveFromRolesAsync(It.IsAny<User>(), emptyRoleList), Times.Once);
        }

        [Fact]
        public void Given_UserNameThatNotExistInDbAndRoleEditDtoForUser_When_EditRoles_ThenReturn_BadRequestWithMessage()
        {
            //Arrange
            var userName = "NotExistUser";
            var roleEditDto = new EditingRoleDto()
            {
                RoleNames = new string[] { "Customer", "Moderator" }
            };

            _mockedUserManager.Setup(s => s.FindByNameAsync(userName))
                        .Returns(Task.FromResult<User>(null));

            //Act
            var result = _cut.EditRoles(userName, roleEditDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("User not found");

            _mockedUserManager.Verify(v => v.FindByNameAsync(userName), Times.Once);

        }

        [Fact]
        public void Given_UserNameAndRoleEditDtoForUser_When_EditRoles_ThenReturn_BadRequestWithMessage_BecauseErrorOccuredDuringAddingRole()
        {
            //Arrange
            var userName = "TestUser";
            var roleEditDto = new EditingRoleDto()
            {
                RoleNames = new string[] { "Customer", "Moderator" }
            };

            _mockedUserManager.Setup(s => s.FindByNameAsync(userName))
                        .Returns(Task.FromResult<User>(new User() { UserName = userName }));

            _mockedUserManager.Setup(s => s.GetRolesAsync(It.IsAny<User>()))
                            .Returns(Task.FromResult<IList<string>>(new List<string>()));

            _mockedUserManager.Setup(s => s.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
                        .Returns(Task.FromResult(IdentityResult.Failed()));

            //Act
            var result = _cut.EditRoles(userName, roleEditDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Fail during adding roles to specified user");

            _mockedUserManager.Verify(v => v.FindByNameAsync(userName), Times.Once);
            _mockedUserManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Once);
            _mockedUserManager.Verify(v => v.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            _mockedUserManager.Verify(v => v.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Never);

        }

        [Fact]
        public void Given_UserNameAndRoleEditDtoForUser_When_EditRoles_ThenReturn_BadRequestWithMessage_BecauseErrorOccuredDuringRemovingRole()
        {
            //Arrange
            var userName = "TestUser";
            var roleEditDto = new EditingRoleDto()
            {
                RoleNames = new string[] { "Customer", "Moderator" }
            };

            _mockedUserManager.Setup(s => s.FindByNameAsync(userName))
                        .Returns(Task.FromResult<User>(new User() { UserName = userName }));

            _mockedUserManager.Setup(s => s.GetRolesAsync(It.IsAny<User>()))
                            .Returns(Task.FromResult<IList<string>>(new List<string>()));

            _mockedUserManager.Setup(s => s.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
                        .Returns(Task.FromResult(IdentityResult.Success));

            _mockedUserManager.Setup(s => s.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
                        .Returns(Task.FromResult(IdentityResult.Failed()));

            //Act
            var result = _cut.EditRoles(userName, roleEditDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Fail during removing roles to specified user");

            _mockedUserManager.Verify(v => v.FindByNameAsync(userName), Times.Once);
            _mockedUserManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Once);
            _mockedUserManager.Verify(v => v.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            _mockedUserManager.Verify(v => v.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Once);

        }


        [Fact]
        public void Given_ProductParamsFromQuery_When_GetProductsForModeration_ThenReturn_OkWithListOfProductForModerationDto()
        {
            //Arrange
            var productParams = new ProductParameters();

            int pageNumber = productParams.PageNumber;
            int pageSize = productParams.PageSize;

            var productModerationDto = Data.ProductModerationDto();

            var expectedPaginationHeader = JsonConvert.SerializeObject(new { currentPage = pageNumber, itemsPerPage = pageSize, totalItems = productModerationDto.Count, totalPages = 1 });

            _mockedUnitOfWork.Setup(s => s.Product.GetProductsForModerationAsync(productParams))
                            .Returns(Task.FromResult(new PagedList<ProductModerationDto>(productModerationDto, productModerationDto.Count,
                                    pageNumber, pageSize)));


            //Act
            var result = _cut.GetProductsForModeration(productParams).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();

            _cut.HttpContext.Response.Headers["Pagination"].Should().BeEquivalentTo(expectedPaginationHeader);

            result.As<OkObjectResult>().Value.Should().NotBeNull();

            result.As<OkObjectResult>().Value.Should().BeOfType<List<ProductModerationDto>>();

            _mockedUnitOfWork.Verify(v => v.Product.GetProductsForModerationAsync(productParams), Times.Once);


        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(-1, -1)]
        public void Given_ProductParamsFromQueryThatAreLessThen1_When_GetProductsForModeration_ThenReturn_BadRequestWithMessage(int pageNumber, int pageSize)
        {
            //Arrange
            var productParams = new ProductParameters()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            //Act
            var result = _cut.GetProductsForModeration(productParams).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("PageNumber or PageSize is less then 1");

            _mockedUnitOfWork.Verify(v => v.Product.GetProductsForModerationAsync(productParams), Times.Never);
        }

        [Fact]
        public void Given_ProductParamsFromQuery_When_GetProductsForModeration_ThenReturn_NotFound()
        {
            //Arrange
            var productParams = new ProductParameters();

            _mockedUnitOfWork.Setup(s => s.Product.GetProductsForModerationAsync(productParams))
                            .ReturnsAsync((PagedList<ProductModerationDto>)(null));

            //Act
            var result = _cut.GetProductsForModeration(productParams).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.Product.GetProductsForModerationAsync(productParams), Times.Once);
        }

        [Fact]
        public void Given_ProductParamsFromQuery_When_GetProductsForModeration_ThenReturn_BadRequestWithMessage_BecausePaginationParametersNotSetProperly()
        {
            //Arrange
            var productParams = new ProductParameters();
            int pageNumber = productParams.PageNumber;
            int pageSize = productParams.PageSize;

            var productModerationDto = Data.ProductModerationDto();

            var pagedListForMock = new PagedList<ProductModerationDto>(productModerationDto,
                    productModerationDto.Count, pageNumber, pageSize);

            _mockedUnitOfWork.Setup(s => s.Product.GetProductsForModerationAsync(productParams))
                            .Returns(Task.FromResult(pagedListForMock))
                            .Callback(() =>
                            {
                                pagedListForMock.PageSize = 0;
                                pagedListForMock.TotalCount = -1;
                                pagedListForMock.TotalPages = 0;
                                pagedListForMock.CurrentPage = 0;
                            });

            //Act
            var result = _cut.GetProductsForModeration(productParams).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Pagination parameters wasn't set properly");

            _mockedUnitOfWork.Verify(v => v.Product.GetProductsForModerationAsync(productParams), Times.Once);
        }


        [Fact]
        public void Given_ProductId_When_GetProductForEdit_ThenReturn_OkWithProductToEditDto()
        {
            //Arrange
            int productId = 1;

            _mockedUnitOfWork.Setup(s => s.Requirements.GetRequirementsForProductAsync(productId))
                            .ReturnsAsync(new EditingRequirementsDto());

            _mockedUnitOfWork.Setup(s => s.Picture.GetPicturesForProduct(productId))
                            .ReturnsAsync(new List<Picture>());

            _mockedUnitOfWork.Setup(s => s.Product.GetProductToEditAsync(It.IsAny<EditingRequirementsDto>(),
                                    It.IsAny<IEnumerable<Picture>>(), productId))
                            .ReturnsAsync(new EditingProductDto());


            //Act
            var result = _cut.GetProductForEdit(productId).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeOfType<EditingProductDto>();
            result.As<OkObjectResult>().Value.Should().NotBeNull();

            _mockedUnitOfWork.Verify(s => s.Requirements.GetRequirementsForProductAsync(productId), Times.Once);

            _mockedUnitOfWork.Verify(s => s.Picture.GetPicturesForProduct(productId), Times.Once);

            _mockedUnitOfWork.Verify(s => s.Product.GetProductToEditAsync(It.IsAny<EditingRequirementsDto>(),
                                    It.IsAny<IEnumerable<Picture>>(), productId), Times.Once);
        }

        [Fact]
        public void Given_ProductId_When_GetProductForEdit_ThenReturn_BadRequestWithMessage_BecauseProductDoNotHaveRequirements()
        {
            //Arrange
            int productId = 1;

            _mockedUnitOfWork.Setup(s => s.Requirements.GetRequirementsForProductAsync(productId))
                            .ReturnsAsync((EditingRequirementsDto)(null));


            //Act
            var result = _cut.GetProductForEdit(productId).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Error occured during retrieving requirements for selected product");

            _mockedUnitOfWork.Verify(s => s.Requirements.GetRequirementsForProductAsync(productId), Times.Once);


        }

        [Fact]
        public void Given_ProductId_When_GetProductForEdit_ThenReturn_BadRequestWithMessage_BecauseProductToEditNotReturned()
        {
            //Arrange
            int productId = 1;

            _mockedUnitOfWork.Setup(s => s.Requirements.GetRequirementsForProductAsync(productId))
                            .ReturnsAsync(new EditingRequirementsDto());

            _mockedUnitOfWork.Setup(s => s.Picture.GetPicturesForProduct(productId))
                            .ReturnsAsync(new List<Picture>());

            _mockedUnitOfWork.Setup(s => s.Product.GetProductToEditAsync(It.IsAny<EditingRequirementsDto>(),
                                    It.IsAny<IEnumerable<Picture>>(), productId))
                            .ReturnsAsync((EditingProductDto)(null));


            //Act
            var result = _cut.GetProductForEdit(productId).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Error occured during retrieving product");

            _mockedUnitOfWork.Verify(s => s.Requirements.GetRequirementsForProductAsync(productId), Times.Once);

            _mockedUnitOfWork.Verify(s => s.Picture.GetPicturesForProduct(productId), Times.Once);

            _mockedUnitOfWork.Verify(s => s.Product.GetProductToEditAsync(It.IsAny<EditingRequirementsDto>(),
                                    It.IsAny<IEnumerable<Picture>>(), productId), Times.Once);


        }


        [Fact]
        public void Given_ProductForCreationDto_When_CreateProduct_ThenReturn_CreatedAtRoute()
        {
            //Arrange
            var productCreationDto = new ProductCreationDto() { CategoryId = 2 };

            _mockedUnitOfWork.Setup(s => s.Category.GetAsync(It.IsAny<int>()))
                                .ReturnsAsync(new Category());

            _mockedUnitOfWork.Setup(s => s.Product.ScaffoldProductForCreationAsync(productCreationDto, It.IsAny<Requirements>(), It.IsAny<Category>()))
                                .ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);


            //Act
            var result = _cut.CreateProduct(productCreationDto).Result;

            //Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
            result.As<CreatedAtRouteResult>().Value.Should().NotBeNull();

            _mockedUnitOfWork.Verify(v => v.Category.GetAsync(It.IsAny<int>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.ScaffoldProductForCreationAsync(productCreationDto, It.IsAny<Requirements>(), It.IsAny<Category>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.Add(It.IsAny<Product>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);


        }

        [Fact]
        public void Given_ProductForCreationDtoThatIsNull_When_CreateProduct_ThenReturn_BadRequestWithMessage()
        {
            //Arrange

            //Act
            var result = _cut.CreateProduct(null).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Sended null product");

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Given_ProductForCreationDtoThatCategoryIdIsLessThen1_When_CreateProduct_ThenReturn_BadRequestWithMessage(int categoryId)
        {
            //Arrange
            var productForCreationDto = new ProductCreationDto() { CategoryId = categoryId };
            //Act
            var result = _cut.CreateProduct(productForCreationDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Category wasn't passed or passed bad CategoryId");

        }

        [Fact]
        public void Given_ProductForCreationDto_When_CreateProduct_ThenReturn_BadRequestWithMessage_BecauseProductWasntScaffoldedProperly()
        {
            //Arrange
            var productForCreationDto = new ProductCreationDto() { CategoryId = 2 };

            _mockedUnitOfWork.Setup(s => s.Category.GetAsync(It.IsAny<int>()))
                                .ReturnsAsync(new Category());

            _mockedUnitOfWork.Setup(s => s.Product.ScaffoldProductForCreationAsync(productForCreationDto, It.IsAny<Requirements>(), It.IsAny<Category>()))
                                .ReturnsAsync((Product)null);


            //Act
            var result = _cut.CreateProduct(productForCreationDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Something went wrong during creating Product");

            _mockedUnitOfWork.Verify(v => v.Category.GetAsync(It.IsAny<int>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.ScaffoldProductForCreationAsync(productForCreationDto, It.IsAny<Requirements>(), It.IsAny<Category>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.Add(It.IsAny<Product>()), Times.Never);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Never);


        }

        [Fact]
        public void Given_ProductForCreationDto_When_CreateProduct_ThenReturn_BadRequestWithMessage_BecauseProductWasntSavedInDb()
        {
            //Arrange
            var productForCreationDto = new ProductCreationDto() { CategoryId = 2 };

            _mockedUnitOfWork.Setup(s => s.Category.GetAsync(It.IsAny<int>()))
                                .ReturnsAsync(new Category());

            _mockedUnitOfWork.Setup(s => s.Product.ScaffoldProductForCreationAsync(productForCreationDto, It.IsAny<Requirements>(), It.IsAny<Category>()))
                                .ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(false);


            //Act
            var result = _cut.CreateProduct(productForCreationDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Error occured during Saving Product");

            _mockedUnitOfWork.Verify(v => v.Category.GetAsync(It.IsAny<int>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.ScaffoldProductForCreationAsync(productForCreationDto, It.IsAny<Requirements>(), It.IsAny<Category>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.Add(It.IsAny<Product>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);


        }

        [Fact]
        public void Given_ProductIdAndProductEditDto_When_EditProduct_ThenReturn_CreatedAtRoute()
        {
            //Arrange
            int productId = 1;
            var productEditDto = new ProductEditDto();

            _mockedUnitOfWork.Setup(s => s.Product.GetAsync(productId))
                        .ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.Category.FindAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                        .ReturnsAsync(new Category());

            _mockedUnitOfWork.Setup(s => s.Product.ScaffoldProductForEditAsync(productId, productEditDto, It.IsAny<Requirements>(),
                             It.IsAny<Category>(), It.IsAny<Product>()))
                        .ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);

            //Act
            var result = _cut.EditProduct(productId, productEditDto).Result;

            //Assert
            result.Should().BeOfType<CreatedAtRouteResult>();

            _mockedUnitOfWork.Verify(v => v.Product.GetAsync(productId), Times.Exactly(2));

            _mockedUnitOfWork.Verify(v => v.Category.FindAsync(It.IsAny<Expression<Func<Category, bool>>>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.ScaffoldProductForEditAsync(productId, productEditDto, It.IsAny<Requirements>(),
                             It.IsAny<Category>(), It.IsAny<Product>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);

        }

        [Fact]
        public void Given_ProductIdAndProductEditDtoThatIsNull_When_EditProduct_ThenReturn_BadRequestWithMessage()
        {
            //Arrange
            int productId = 1;

            //Act
            var result = _cut.EditProduct(productId, null).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Sended null product");

        }

        [Fact]
        public void Given_ProductIdThatNotExistInDbAndProductEditDto_When_EditProduct_ThenReturn_BadRequestWithMessage()
        {
            //Arrange
            int productId = 1;
            var productEditDto = new ProductEditDto();

            _mockedUnitOfWork.Setup(s => s.Product.GetAsync(productId)).ReturnsAsync((Product)null);

            //Act
            var result = _cut.EditProduct(productId, productEditDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be($"Product with Id:{productId} not found");

        }

        [Fact]
        public void Given_ProductIdAndProductEditDto_When_EditProduct_ThenReturn_BadRequestWithMessage_BecauseProductWasntSavedInDb()
        {
            //Arrange
            int productId = 1;
            var productEditDto = new ProductEditDto();

            _mockedUnitOfWork.Setup(s => s.Product.GetAsync(productId))
                        .ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.Category.FindAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                        .ReturnsAsync(new Category());

            _mockedUnitOfWork.Setup(s => s.Product.ScaffoldProductForEditAsync(productId, productEditDto, It.IsAny<Requirements>(),
                             It.IsAny<Category>(), It.IsAny<Product>()))
                        .ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(false);

            //Act
            var result = _cut.EditProduct(productId, productEditDto).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Fail occured during editing Product");

            _mockedUnitOfWork.Verify(v => v.Product.GetAsync(productId), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Category.FindAsync(It.IsAny<Expression<Func<Category, bool>>>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.ScaffoldProductForEditAsync(productId, productEditDto, It.IsAny<Requirements>(),
                             It.IsAny<Category>(), It.IsAny<Product>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);

        }

        [Fact]
        public void Given_ProductId_When_DeleteProduct_ThenRetrun_NoContentStatus()
        {
            //Arrange
            int productId = 1;

            _mockedUnitOfWork.Setup(s => s.Product.GetAsync(productId))
            .ReturnsAsync(new Product()
            {
                Pictures = new List<Picture>()
                {
                    new Picture()
                    {
                        Id = 1,
                    },
                    new Picture()
                    {
                        Id = 2,
                    },
                    new Picture()
                    {
                        Id = 3,
                    }
                }
            });

            _mockedUnitOfWork.Setup(s => s.Picture.Delete(It.IsAny<Picture>()));
            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);

            //Act
            var result = _cut.DeleteProduct(productId).Result;

            //Assert
            result.Should().BeOfType<NoContentResult>();


            _mockedUnitOfWork.Verify(v => v.Product.GetAsync(productId), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Picture.Delete(It.IsAny<Picture>()), Times.Exactly(3));

            _mockedUnitOfWork.Verify(v => v.Product.Delete(It.IsAny<Product>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);
        }

        [Fact]
        public void Given_ProductIdThatNotExistInDb_When_DeleteProduct_ThenRetrun_BadRequestWithMessage()
        {
            //Arrange
            int productId = 1;

            _mockedUnitOfWork.Setup(s => s.Product.GetAsync(productId))
                .ReturnsAsync((Product)null);

            //Act
            var result = _cut.DeleteProduct(productId).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Product for that Id doesn't exist");

            _mockedUnitOfWork.Verify(v => v.Product.GetAsync(productId), Times.Once);

        }

        [Fact]
        public void Given_ProductId_When_DeleteProduct_ThenRetrun_BadRequestWithMessage_BecauseErrorOccuredDuringSavingInDbThatProductWasDeleted()
        {
            //Arrange
            int productId = 1;

            _mockedUnitOfWork.Setup(s => s.Product.GetAsync(productId))
                .ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.Picture.Delete(It.IsAny<Picture>()));
            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(false);


            //Act
            var result = _cut.DeleteProduct(productId).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Something went wrong during deleting product");

            _mockedUnitOfWork.Verify(v => v.Product.GetAsync(productId), Times.Once);

            _mockedUnitOfWork.Verify(v => v.Product.Delete(It.IsAny<Product>()), Times.Once);

            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);


        }

        [Fact]
        public void Given_ProductId_When_DeleteProduct_CheckingIfProductThatDoNotHavePhotos_NotThrowException()
        {
            //Arrange
            int productId = 1;

            _mockedUnitOfWork.Setup(s => s.Product.GetAsync(productId))
                .ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.Picture.Delete(It.IsAny<Picture>()));
            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);


            //Act
            _cut.Invoking(a => a.DeleteProduct(productId)).Should().NotThrowAsync();



        }

        [Fact]
        public void Given_None_When_GetCategories_ThenRetrun_OkStatusWithCategoriesToReturnDto()
        {
            //Arrange           
            _mockedUnitOfWork.Setup(s => s.Category.GetAllOrderedByAsync(It.IsAny<Expression<Func<Category, int>>>()))
                            .ReturnsAsync(new List<Category>() { new Category() });
            //Act
            var result = _cut.GetCategories().Result;


            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull();
            result.As<OkObjectResult>().Value.Should().BeOfType<List<CategoryToReturnDto>>();

            _mockedUnitOfWork.Verify(v => v.Category.GetAllOrderedByAsync(It.IsAny<Expression<Func<Category, int>>>()), Times.Once);



        }

        [Fact]
        public void Given_None_When_GetCategories_ThenRetrun_NotFound_Because_CategoriesFromDbIsNull()
        {
            //Arrange           
            _mockedUnitOfWork.Setup(s => s.Category.GetAllOrderedByAsync(It.IsAny<Expression<Func<Category, int>>>()))
                            .ReturnsAsync((IEnumerable<Category>)null);
            //Act
            var result = _cut.GetCategories().Result;


            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.Category.GetAllOrderedByAsync(It.IsAny<Expression<Func<Category, int>>>()), Times.Once);

        }


        [Fact]
        public void Given_None_When_GetCategories_ThenRetrun_NotFound_Because_CategoriesFromDbAreEmpty()
        {
            //Arrange 
            var emptyCategoriesList = new List<Category>();

            _mockedUnitOfWork.Setup(s => s.Category.GetAllOrderedByAsync(It.IsAny<Expression<Func<Category, int>>>()))
                            .ReturnsAsync(emptyCategoriesList);
            //Act
            var result = _cut.GetCategories().Result;


            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.Category.GetAllOrderedByAsync(It.IsAny<Expression<Func<Category, int>>>()), Times.Once);

        }

        [Fact]
        public void Given_None_When_GetSubCategories_ThenRetrun_OkStatusWithSubCategoriesToReturnDto()
        {
            //Arrange           
            _mockedUnitOfWork.Setup(s => s.SubCategory.GetAllAsync())
                            .ReturnsAsync(new List<SubCategory>() { new SubCategory() });
            //Act
            var result = _cut.GetSubCategories().Result;


            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull();
            result.As<OkObjectResult>().Value.Should().BeOfType<List<SubCategoryReturnDto>>();

            _mockedUnitOfWork.Verify(v => v.SubCategory.GetAllAsync(), Times.Once);

        }

        [Fact]
        public void Given_None_When_GetSubCategories_ThenRetrun_NotFound_Because_SubCategoriesFromDbIsNull()
        {
            //Arrange           
            _mockedUnitOfWork.Setup(s => s.SubCategory.GetAllAsync())
                            .ReturnsAsync((IEnumerable<SubCategory>)null);
            //Act
            var result = _cut.GetSubCategories().Result;


            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.SubCategory.GetAllAsync(), Times.Once);

        }

        [Fact]
        public void Given_None_When_GetSubCategories_ThenRetrun_NotFound_Because_SubCategoriesFromDbAreEmpty()
        {
            //Arrange
            var emptySubCategoriesList = new List<SubCategory>();

            _mockedUnitOfWork.Setup(s => s.SubCategory.GetAllAsync())
                            .ReturnsAsync(emptySubCategoriesList);
            //Act
            var result = _cut.GetSubCategories().Result;


            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.SubCategory.GetAllAsync(), Times.Once);

        }

        [Fact]
        public void Given_None_When_GetLanguages_ThenRetrun_OkStatusWithLanguagesToReturnDto()
        {
            //Arrange           
            _mockedUnitOfWork.Setup(s => s.Language.GetAllOrderedByAsync(It.IsAny<Expression<Func<Language, int>>>()))
                            .ReturnsAsync(new List<Language>() { new Language() });
            //Act
            var result = _cut.GetLanguages().Result;


            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull();
            result.As<OkObjectResult>().Value.Should().BeOfType<List<ReturnLanguagesDto>>();


            _mockedUnitOfWork.Verify(v => v.Language.GetAllOrderedByAsync(It.IsAny<Expression<Func<Language, int>>>()), Times.Once);

        }

        [Fact]
        public void Given_None_When_GetLanguages_ThenRetrun_NotFound_Because_LanguagesFromDbIsNull()
        {
            //Arrange           
            _mockedUnitOfWork.Setup(s => s.Language.GetAllOrderedByAsync(It.IsAny<Expression<Func<Language, int>>>()))
                            .ReturnsAsync((IEnumerable<Language>)null);
            //Act
            var result = _cut.GetLanguages().Result;


            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.Language.GetAllOrderedByAsync(It.IsAny<Expression<Func<Language, int>>>()), Times.Once);

        }

        [Fact]
        public void Given_None_When_GetLanguages_ThenRetrun_NotFound_Because_LanguagesFromDbAreEmpty()
        {
            //Arrange
            var emptyLanguagesList = new List<Language>();

            _mockedUnitOfWork.Setup(s => s.Language.GetAllOrderedByAsync(It.IsAny<Expression<Func<Language, int>>>()))
                            .ReturnsAsync(emptyLanguagesList);
            //Act
            var result = _cut.GetLanguages().Result;


            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.Language.GetAllOrderedByAsync(It.IsAny<Expression<Func<Language, int>>>()), Times.Once);

        }

        [Fact]
        public void Given_CategoryId_When_GetCategory_ThenReturn_OkStatusWithCategoryToReturnDto()
        {
            //Arrange
            var categoryId = 1;

            _mockedUnitOfWork.Setup(s => s.Category.GetAsync(categoryId)).ReturnsAsync(new Category());

            //Act
            var result = _cut.GetCategory(categoryId).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull();
            result.As<OkObjectResult>().Value.Should().BeOfType<CategoryToReturnDto>();


            _mockedUnitOfWork.Verify(v => v.Category.GetAsync(categoryId), Times.Once);
        }

        [Fact]
        public void Given_CategoryId_When_GetCategory_ThenReturn_NotFound()
        {
            //Arrange
            var categoryId = 1;

            _mockedUnitOfWork.Setup(s => s.Category.GetAsync(categoryId)).ReturnsAsync((Category)null);

            //Act
            var result = _cut.GetCategory(categoryId).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();

            _mockedUnitOfWork.Verify(v => v.Category.GetAsync(categoryId), Times.Once);
        }

        [Fact]
        public void Given_ProductIdAndQuantity_When_EditStockForProduct_CaseWhenProductHaveStock_ThenReturn_OkWithEditedStock()
        {
            //Arrange
            int productId = 1;
            int quantity = 100;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithStockOnlyAsync(productId)).ReturnsAsync(new Product() { Stock = new Stock() });

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);

            _mockedUnitOfWork.Setup(s => s.Stock.GetByProductId(productId)).ReturnsAsync(new Stock());

            //Act
            var result = _cut.EditStockForProduct(productId, quantity).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should()
                        .BeOfType<Stock>()
                        .And.NotBeNull();

            _mockedUnitOfWork.Verify(v => v.Product.GetWithStockOnlyAsync(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);
            _mockedUnitOfWork.Verify(v => v.Stock.GetByProductId(productId), Times.Once);
        }

        [Fact]
        public void Given_ProductIdAndQuantity_When_EditStockForProduct_CaseWhenProductDoNotHaveStock_ThenReturn_OkWithEditedStock()
        {
            //Arrange
            int productId = 1;
            int quantity = 100;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithStockOnlyAsync(productId)).ReturnsAsync(new Product() { Stock = null });

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(true);

            _mockedUnitOfWork.Setup(s => s.Stock.GetByProductId(productId)).ReturnsAsync(new Stock());

            //Act
            var result = _cut.EditStockForProduct(productId, quantity).Result;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should()
                        .BeOfType<Stock>()
                        .And.NotBeNull();

            _mockedUnitOfWork.Verify(v => v.Product.GetWithStockOnlyAsync(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);
            _mockedUnitOfWork.Verify(v => v.Stock.GetByProductId(productId), Times.Once);
        }

        [Fact]
        public void Given_ProductIdThatNotExistAndQuantity_When_EditStockForProduct_ThenReturn_BadRequestWithMessage()
        {
            //Arrange
            int productIdThatNotExist = 1000;
            int quantity = 100;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithStockOnlyAsync(productIdThatNotExist)).ReturnsAsync((Product)null);

            //Act
            var result = _cut.EditStockForProduct(productIdThatNotExist, quantity).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                            .Should().Be("Product for That Id dont exist");


            _mockedUnitOfWork.Verify(v => v.Product.GetWithStockOnlyAsync(productIdThatNotExist), Times.Once);
        }

        [Fact]
        public void Given_ProductIdAndQuantityThatIsSameAsProductQty_When_EditStockForProduct_ThenReturn_BadRequestWithMessage()
        {
            //Arrange
            int productId = 1;
            int sameQuantityAsProductQty = 100;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithStockOnlyAsync(productId)).ReturnsAsync(new Product() { Stock = new Stock() { Quantity = sameQuantityAsProductQty } });

            //Act
            var result = _cut.EditStockForProduct(productId, sameQuantityAsProductQty).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                            .Should().Be("Passed same quantity as product already have");


            _mockedUnitOfWork.Verify(v => v.Product.GetWithStockOnlyAsync(productId), Times.Once);
        }

        [Fact]
        public void Given_ProductIdAndQuantity_When_EditStockForProduct_ThenReturn_BadRequestWithMessage_BecauseErrorOccuredDuringSaving()
        {
            //Arrange
            int productId = 1;
            int quantity = 100;

            _mockedUnitOfWork.Setup(s => s.Product.GetWithStockOnlyAsync(productId)).ReturnsAsync(new Product());

            _mockedUnitOfWork.Setup(s => s.SaveAsync()).ReturnsAsync(false);

            //Act
            var result = _cut.EditStockForProduct(productId, quantity).Result;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                            .Should().Be("Something went wrong during saving Stock for Product");

            _mockedUnitOfWork.Verify(v => v.Product.GetWithStockOnlyAsync(productId), Times.Once);
            _mockedUnitOfWork.Verify(v => v.SaveAsync(), Times.Once);
        }
    }
}
