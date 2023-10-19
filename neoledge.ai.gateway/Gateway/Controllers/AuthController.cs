using Gateway.Models.Presenter;
using Gateway.Services;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IdentityResult> RegisterUser([FromForm] RegisterUser registerUser)
        {
            return await _authService.RegisterUser(registerUser);
        }

        [HttpPost("Login")]
        public async Task<string> Login([FromForm] LoginUser user)
        {
            return await _authService.LoginUser(user);
        }


    }
}
