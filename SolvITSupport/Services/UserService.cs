using Microsoft.AspNetCore.Identity;
using SolvITSupport.Models;

namespace SolvITSupport.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> AutenticarAsync(string email, string senha)
        {
            // O false nos parâmetros é para não bloquear a conta em caso de falha
            return await _signInManager.PasswordSignInAsync(email, senha, isPersistent: false, lockoutOnFailure: false);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string senha)
        {
            // O UserManager trata de guardar a senha de forma segura (hash)
            return await _userManager.CreateAsync(user, senha);
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
    }
}