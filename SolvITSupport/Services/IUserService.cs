using Microsoft.AspNetCore.Identity;
using SolvITSupport.Models;

namespace SolvITSupport.Services
{
    public interface IUserService
    {
        Task<SignInResult> AutenticarAsync(string email, string senha);
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string senha);
    }
}