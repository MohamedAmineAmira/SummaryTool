﻿using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Identity;

namespace Gateway.Services
{
    public interface IAuthService
    {
        Task<string> GenerateTokenString(LoginUser user);
        Task<string> LoginUser(LoginUser loginUser);
        Task<IdentityResult> RegisterUser(RegisterUser registerUser);
    }
}