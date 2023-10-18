using Gateway.Models.Presenter;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUser registerUser)
        {
            if (await _authService.RegisterUser(registerUser))
            {
                return Ok("Successfuly done");
            }
            return BadRequest("Something went false");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _authService.LoginUser(user))
            {
                return Ok("Done");
            }
            return BadRequest();
        }


    }
}
