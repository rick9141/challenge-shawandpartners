using Microsoft.AspNetCore.Mvc;
using ShawAndPartners.Application.Contracts.Services.v1;
using ShawAndPartners.Application.DTOs;
using ShawAndPartners.Domain.Entities.v1;

namespace ShawAndPartners.API.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Search for users by name, city, country or favorite sport.
        /// </summary>
        /// <param name="q">The search query string.</param>
        /// <returns>A list of users that match the search criteria.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
                return BadRequest("Search term is required.");

            var results = await _userService.SearchAsync(q);
            return Ok(results);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns> A list of all users.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound("Users not found.");
            }

            return Ok(users);
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userUpdateDto"></param>
        /// <returns> A message indicating whether the user was updated successfully or not.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateUser(string name, [FromBody] UserUpdateDto userUpdateDto)
        {
            var wasUpdated = await _userService.UpdateUserAsync(name, userUpdateDto);

            if (!wasUpdated)
            {
                return NotFound($"User with name: {name} not found or no changes were made.");
            }

            return Ok("User updated successfully.");
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="name"></param>
        /// <returns> A message indicating whether the user was deleted successfully or not.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteUser(string name)
        {
            var result = await _userService.DeleteByNameAsync(name);
            if (result == 0)
            {
                return NotFound($"User with name: {name} not found.");
            }

            return Ok("User deleted successfully.");
        }
    }
}