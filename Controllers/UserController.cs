using api_teszt.Dtos;
using api_teszt.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_teszt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetUsers();
            if (users.Any())
            {
                return Ok(users.Select(user => new UserDto(user)).ToList());
            }
            
            return NotFound();
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var result = await _userService.GetUserByEmail(email);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(new UserDto(result));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto user)
        {
            var result = await _userService.CreateUser(user);
            if (user is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetUserByEmail), new {email = result.email}, new UserDto(result));
        }

        [HttpPut("{email}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string email, [FromBody] UpdateUserDto user)
        {
            var result = await _userService.UpdateUser(user, email);
            if (result is null) return NotFound();
            return Ok(new UserDto(result));  
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult<bool>> DeleteUser(string email)
        {
            var result = await _userService.DeleteUser(email);
            if (result is false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
