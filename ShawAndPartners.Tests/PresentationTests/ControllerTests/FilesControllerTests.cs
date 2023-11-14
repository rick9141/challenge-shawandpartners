using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShawAndPartners.API.Controllers;
using ShawAndPartners.Application.Contracts.Services.v1;
using System.Text;
using Xunit;

namespace ShawAndPartners.Tests.PresentationTests.ControllerTests
{
    /// <summary>
    /// Tests carried out following the AAA standard.
    /// </summary>
    public class FilesControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly FilesController _controller;
        private readonly IFormFile _mockFile;

        public FilesControllerTests()
        {
            _mockUserService = new Mock<IUserService>();

            _mockFile = Mock.Of<IFormFile>(file =>
                file.Length == 1024 && file.FileName == "users.csv" &&
                file.OpenReadStream() == new MemoryStream(Encoding.UTF8.GetBytes("test data")));

            _controller = new FilesController(_mockUserService.Object);
        }

        [Fact]
        public async Task Upload_ReturnsBadRequest_WhenFileIsNull()
        {
            IFormFile nullFile = null;

            var result = await _controller.Upload(nullFile);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file submitted or the file is empty.", badRequestResult.Value);
        }

        [Fact]
        public async Task Upload_ReturnsBadRequest_WhenFileIsEmpty()
        {
            var emptyFile = Mock.Of<IFormFile>(file => file.Length == 0);

            var result = await _controller.Upload(emptyFile);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file submitted or the file is empty.", badRequestResult.Value);
        }

        [Fact]
        public async Task Upload_ReturnsOk_WhenFileIsUploaded()
        {

            var result = await _controller.Upload(_mockFile);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("File uploaded successfully.", okResult.Value);
        }
    }

}
