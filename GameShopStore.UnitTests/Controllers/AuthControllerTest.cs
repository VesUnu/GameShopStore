using Castle.Core.Configuration;
using FluentAssertions;
using GameShopStore.Controllers;
using GameShopStore.Core.Dtos.UserDtos;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameShopStore.UnitTests.Controllers
{
    public class AuthControllerTest : UnitTestBase, IDisposable
    {
        [Fact]
        public void Given_AnyButPassedUserForRegisterDtoOnlyForCallMethod_When_Register_ThenRetrun_CreatedStatus()
        {
            //Arrange
            var userForRegisterDto = new UserRegisterDto()
            {
                Username = "David",
                Surname = "Example",
                Email = "david.example@example.com",
                Password = "Password"
            };

            string customerRole = "Customer";

            _mockedUserManager.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(IdentityResult.Success));

            _mockedUserManager.Setup(s => s.AddToRoleAsync(It.IsAny<User>(), customerRole))
                        .Returns(Task.FromResult(IdentityResult.Success));

            var cut = new AuthController(_mockedConfig.Object, _mockedUserManager.Object, _mockedSignInManager.Object, _mockedJwtTokenHelper.Object);

            //Act
            var result = cut.Register(userForRegisterDto).Result;



            //Assert
            result.Should().BeOfType<OkObjectResult>();

            _mockedUserManager.Verify(v => v.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            _mockedUserManager.Verify(v => v.AddToRoleAsync(It.IsAny<User>(), customerRole), Times.Once);
        }

        [Fact]
        public void Given_AnyButPassedUserForRegisterDtoOnlyForCallMethod_When_Register_ThenRetrun_BadRequestSatus_BecauseFailOccuredDuringAddingRole()
        {
            //Arrange
            var userForRegisterDto = new UserRegisterDto()
            {
                Username = "David",
                Surname = "Example",
                Email = "david.example@example.com",
                Password = "Password"
            };

            _mockedUserManager.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(IdentityResult.Success));

            _mockedUserManager.Setup(s => s.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(new IdentityResult()));

            var cut = new AuthController(_mockedConfig.Object, _mockedUserManager.Object, _mockedSignInManager.Object, _mockedJwtTokenHelper.Object);

            //Act
            var result = cut.Register(userForRegisterDto).Result;



            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            _mockedUserManager.Verify(v => v.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            _mockedUserManager.Verify(v => v.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Given_UserForLoginDto_When_Login_ThenRetrun_OkWithToken()
        {
            //Arrange
            var userForLoginDto = new UserLoginDto()
            {
                Email = "TestUser@example.com",
                Password = "Password"
            };



            _mockedUserManager.Setup(s => s.FindByEmailAsync(userForLoginDto.Email))
                        .ReturnsAsync(new User());

            _mockedSignInManager.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<User>(), userForLoginDto.Password, false))
                                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockedJwtTokenHelper.Setup(s => s.GenerateJwtToken(It.IsAny<User>(), It.IsAny<UserManager<User>>(), (Microsoft.Extensions.Configuration.IConfiguration)It.IsAny<IConfiguration>()))
                                .ReturnsAsync("FakeToken");

            var cut = new AuthController(_mockedConfig.Object, _mockedUserManager.Object, _mockedSignInManager.Object, _mockedJwtTokenHelper.Object);

            //Act
            var result = cut.Login(userForLoginDto).Result;



            //Assert
            result.Should().BeOfType<OkObjectResult>();

            _mockedUserManager.Verify(v => v.FindByEmailAsync(userForLoginDto.Email), Times.Once);

            _mockedSignInManager.Verify(v => v.CheckPasswordSignInAsync(It.IsAny<User>(), userForLoginDto.Password, false), Times.Once);

            _mockedJwtTokenHelper.Verify(v => v.GenerateJwtToken(It.IsAny<User>(), It.IsAny<UserManager<User>>(), (Microsoft.Extensions.Configuration.IConfiguration)It.IsAny<IConfiguration>()), Times.Once);
        }


        [Fact]
        public void Given_UserForLoginDtoThatNotExist_When_Login_ThenRetrun_Unauthorized()
        {
            //Arrange
            var userForLoginDto = new UserLoginDto()
            {
                Email = "EmailThatNotExists@example.com",
                Password = "Password"
            };

            _mockedUserManager.Setup(s => s.FindByEmailAsync(userForLoginDto.Email))
                        .Returns(Task.FromResult<User>(null!));

            var cut = new AuthController(_mockedConfig.Object, _mockedUserManager.Object, _mockedSignInManager.Object, _mockedJwtTokenHelper.Object);

            //Act
            var result = cut.Login(userForLoginDto).Result;



            //Assert
            result.Should().BeOfType<UnauthorizedResult>();

            _mockedUserManager.Verify(v => v.FindByEmailAsync(userForLoginDto.Email), Times.Once);
            // _startup.mockedSignInManager.Verify(v => v.CheckPasswordSignInAsync(It.IsAny<User>(), userForLoginDto.Password, false), Times.Once);
        }

        [Fact]
        public void Given_UserForLoginDtoWithBadPassword_When_Login_ThenRetrun_Unauthorized()
        {
            //Arrange
            var userForLoginDto = new UserLoginDto()
            {
                Email = "peshoman@abv.bg",
                Password = "BadPassword"
            };

            _mockedUserManager.Setup(s => s.FindByEmailAsync(userForLoginDto.Email))
                        .Returns(Task.FromResult<User>(new User() { UserName = userForLoginDto.Email }));

            _mockedSignInManager.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<User>(), userForLoginDto.Password, false))
                        .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));

            var cut = new AuthController(_mockedConfig.Object, _mockedUserManager.Object, _mockedSignInManager.Object, _mockedJwtTokenHelper.Object);

            //Act
            var result = cut.Login(userForLoginDto).Result;



            //Assert
            result.Should().BeOfType<UnauthorizedResult>();

            _mockedUserManager.Verify(v => v.FindByEmailAsync(userForLoginDto.Email), Times.Once);
            _mockedSignInManager.Verify(v => v.CheckPasswordSignInAsync(It.IsAny<User>(), userForLoginDto.Password, false), Times.Once);
        }
    }
}
