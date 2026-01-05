using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.Shared.RCL.DTOs;

namespace TP.Shared.RCL.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDto loginModel);
        Task<bool> RegisterAsync(RegisterDto registerModel);
        Task LogoutAsync();
    }
}