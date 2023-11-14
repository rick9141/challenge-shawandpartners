using Microsoft.AspNetCore.Mvc;
using Moq;
using ShawAndPartners.API.Controllers;
using ShawAndPartners.Application.Contracts.Services.v1;
using ShawAndPartners.Application.DTOs;
using ShawAndPartners.Domain.Entities.v1;
using Xunit;

namespace ShawAndPartners.Tests.PresentationTests.ControllerTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _controller;
        private readonly List<User> _usersList;

        public UsersControllerTests()
        {

            _userServiceMock = new Mock<IUserService>();
            _controller = new UsersController(_userServiceMock.Object);
            _usersList = new List<User>
        {
            new User
            {
                Name = "Luis",
                City = "Guariba",
                Country = "Brazil",
                FavoriteSport = "Esports"
             },
            new User
            {
                Name = "Chris",
                City = "San Francisco",
                Country = "United States",
                FavoriteSport = "Baseball"
            }
        };
        }

        [Fact]
        public async Task Search_WhenCalledWithEmptyQuery_ReturnsBadRequest()
        {
            var result = await _controller.Search("");

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Search_WhenCalledWithValidQuery_ReturnsExpectedUsers()
        {
            var searchTerm = "test";
            _userServiceMock.Setup(service => service.SearchAsync(searchTerm))
                            .ReturnsAsync(_usersList);

            var result = await _controller.Search(searchTerm);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(_usersList.Count, returnedUsers.Count);
        }

        [Fact]
        public async Task GetAllUsers_WhenNoUsersFound_ReturnsNotFound()
        {

            _userServiceMock.Setup(service => service.GetAllAsync())
                            .ReturnsAsync(new List<User>());

            var result = await _controller.GetAllUsers();

            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async Task GetAllUsers_WhenUsersExist_ReturnsAllUsers()
        {
            _userServiceMock.Setup(service => service.GetAllAsync())
                            .ReturnsAsync(_usersList);

            var result = await _controller.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(_usersList.Count, returnedUsers.Count);
        }

        [Fact]
        public async Task UpdateUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            var name = "nonexistent";
            var userUpdateDto = new UserUpdateDto { /* inicialize propriedades */ };
            _userServiceMock.Setup(service => service.UpdateUserAsync(name, userUpdateDto))
                            .ReturnsAsync(false);

            var result = await _controller.UpdateUser(name, userUpdateDto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            var name = "nonexistent";
            _userServiceMock.Setup(service => service.DeleteByNameAsync(name))
                            .ReturnsAsync(0);

            var result = await _controller.DeleteUser(name);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

}
