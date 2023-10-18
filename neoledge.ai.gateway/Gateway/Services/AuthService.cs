using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Identity;

namespace Gateway.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AuthService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> RegisterUser(RegisterUser registerUser)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
            };
            var result = await _userManager.CreateAsync(identityUser, registerUser.Password);
            return result.Succeeded;
        }

        public async Task<bool> LoginUser(LoginUser loginUser)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginUser.UserName);
            if (identityUser == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(identityUser, loginUser.Password);
        }

    }
}
