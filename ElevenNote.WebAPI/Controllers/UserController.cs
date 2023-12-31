using Microsoft.AspNetCore.Mvc;
using ElevenNote.Services.User;
using ElevenNote.Models.User;
using Microsoft.AspNetCore.Authorization;

namespace ElevenNote.WebAPI.Controllers
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

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var registerResult = await _userService.RegisterUserAsync(model);
            
            if (registerResult)
            {
                return Ok("User was registered.");
            }

            return BadRequest("User could not be registered.");
        }
        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int userId)
        {
            var userDetail = await _userService.GetUserByIdAsync(userId);

            if (userDetail is null)
            {
                return NotFound();
            }

            return Ok(userDetail);
        }
    }
}

