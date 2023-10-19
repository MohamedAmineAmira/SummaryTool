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

        public async Task<IdentityResult> RegisterUser(RegisterUser registerUser)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
            };
            var result = await _userManager.CreateAsync(identityUser, registerUser.Password);
            await _userManager.AddToRoleAsync(identityUser, "Admin"); //Moha**333
            return result;
        }

        public async Task<string> LoginUser(LoginUser loginUser)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginUser.Email);
            if (identityUser == null)
            {
                return "Email Not Exists";
            }

            if (!await _userManager.CheckPasswordAsync(identityUser, loginUser.Password))
            {
                return "Password Wrong";
            }
            return "Done";
        }


    }
}
