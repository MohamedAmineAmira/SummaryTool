using Gateway.Models.Presenter;

namespace Gateway.Services
{
    public interface IAuthService
    {
        Task<bool> LoginUser(LoginUser loginUser);
        Task<bool> RegisterUser(RegisterUser registerUser);

    }
}