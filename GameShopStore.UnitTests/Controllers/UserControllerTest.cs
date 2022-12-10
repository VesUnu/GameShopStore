using FluentAssertions;
using GameShopStore.Controllers;
using GameShopStore.Core.Models;
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
    public class UserControllerTest : UnitTestBase, IDisposable
    {
        private readonly UserController _cut;

        public UserControllerTest()
        {
            _cut = new UserController(_mockedUnitOfWork.Object, _mapper, _mockedUserManager.Object);
        }

        [Fact]
        public void Given_None_When_GetUsers_ThenReturn_NotFound()
        {
            //Arrange
            var emptyUsersList = new List<User>();

            _mockedUnitOfWork.Setup(s => s.User.GetAllOrderedByAsync(u => u.Id))
                    .ReturnsAsync(emptyUsersList);
            //Act
            var result = _cut.GetUsers().Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Given_UserId_When_GetUser_ThenReturn_NotFound()
        {
            //Arrange
            int userId = 1;

            _mockedUnitOfWork.Setup(s => s.User.GetAsync(userId))
                    .ReturnsAsync((User)null!);
            //Act
            var result = _cut.GetUserForAccInfo(userId).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
