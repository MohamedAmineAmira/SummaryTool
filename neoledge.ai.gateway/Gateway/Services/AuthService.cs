using Gateway.Models;
using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gateway.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<IdentityResult> RegisterUser(RegisterUser registerUser)
        {
            var identityUser = new ApplicationUser
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
            };
            var result = await _userManager.CreateAsync(identityUser, registerUser.Password);
            await _userManager.AddToRoleAsync(identityUser, "SimpleUser");
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

        public async Task<string> GenerateTokenString(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("userID",identityUser.Id)

            };
            var roles = await _userManager.GetRolesAsync(identityUser);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
            claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }



    }
}
