using Microsoft.AspNetCore.Mvc;
using ShawAndPartners.Application.Contracts.Services.v1;
using ShawAndPartners.Domain.Entities.v1;

namespace ShawAndPartners.API.Controllers
{
    /// <summary>
    /// Files controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userService"></param>
        public FilesController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Upload a CSV file containing users.
        /// </summary>
        /// <param name="file"></param>
        /// <returns> A message informing you of the upload status.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file submitted or the file is empty.");

            await _userService.UploadCsvAsync(file.OpenReadStream());

            return Ok("File uploaded successfully.");
        }
    }
}