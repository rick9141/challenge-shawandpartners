using Moq;
using ShawAndPartners.Application.DTOs;
using ShawAndPartners.Application.Services;
using ShawAndPartners.Domain.Contracts.Repositories;
using ShawAndPartners.Domain.Entities.v1;
using Xunit;

namespace ShawAndPartners.Tests.ApplicationTests.ServicesTests
{
    /// <summary>
    /// Tests carried out following the AAA standard.
    /// </summary>
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;
        private readonly MemoryStream _memoryStream;
        private readonly StreamWriter _streamWriter;
        private readonly string _csvData;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
            _csvData = "name,city,country,favorite_sport\n" +
                       "John Doe,New York,USA,Basketball\n" +
                       "Jane Smith,London,UK,Football";

            _memoryStream = new MemoryStream();
            _streamWriter = new StreamWriter(_memoryStream);
            _streamWriter.Write(_csvData);
            _streamWriter.Flush();
            _memoryStream.Position = 0;
        }

        [Fact]
        public async Task UploadCsvAsync_UploadsUsersCorrectly()
        {
            _mockUserRepository.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string username) =>
                    username == "John Doe" ? new User { Name = "John Doe" } : null);

            _mockUserRepository.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(1);

            _mockUserRepository.Setup(x => x.InsertAsync(It.IsAny<User>()))
                .ReturnsAsync(1);

            await _userService.UploadCsvAsync(_memoryStream);

            _mockUserRepository.Verify(x => x.InsertAsync(It.IsAny<User>()), Times.Once());
            _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once());

            _memoryStream.Close();
            _streamWriter.Close();
        }

        [Fact]
        public async Task SearchAsync_ReturnsMatchedUsers()
        {
            var users = new List<User> { new User { Name = "John Doe" } };
            _mockUserRepository.Setup(repo => repo.SearchAsync("John")).ReturnsAsync(users);

            var result = await _userService.SearchAsync("John");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            var users = new List<User> { new User { Name = "John Doe" }, new User { Name = "Jane Doe" } };
            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            var result = await _userService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateUserAsync_UserExists_UpdatesUser()
        {
            var user = new User { Name = "John Doe", City = "New York" };
            var userDto = new UserUpdateDto { City = "Boston", Country = "USA", FavoriteSport = "Baseball" };
            _mockUserRepository.Setup(repo => repo.GetByNameAsync("John Doe")).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(user)).ReturnsAsync(1);

            var result = await _userService.UpdateUserAsync("John Doe", userDto);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteByNameAsync_UserExists_DeletesUser()
        {
            _mockUserRepository.Setup(repo => repo.DeleteByNameAsync("John Doe")).ReturnsAsync(1);

            var result = await _userService.DeleteByNameAsync("John Doe");

            Assert.Equal(1, result);
        }

        public void Dispose()
        {
            _memoryStream?.Dispose();
            _streamWriter?.Dispose();
        }


    }

}