using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolvITSupport.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace SolvITSupport.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public ManageController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            var profileModel = new ProfileViewModel
            {
                NomeCompleto = user.NomeCompleto,
                Email = user.Email,
                Telefone = user.Telefone,
                Departamento = user.Departamento,
                Bio = user.Bio,
                ProfilePictureUrl = user.Foto
            };

            var model = new SettingsViewModel
            {
                Profile = profileModel,
                Security = new ChangePasswordViewModel(),
                ActiveTab = TempData["ActiveTab"] as string ?? "Perfil"
            };

            if (model.ActiveTab == "Perfil")
            {
                model.Profile.StatusMessage = StatusMessage;
            }
            else
            {
                model.Security.StatusMessage = StatusMessage;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(SettingsViewModel model)
        {
            TempData["ActiveTab"] = "Perfil";

            // --- CORREÇÃO AQUI ---
            // Limpa a validação do formulário de senha, pois ele não foi enviado
            ModelState.Remove("Security.OldPassword");
            ModelState.Remove("Security.NewPassword");
            ModelState.Remove("Security.ConfirmPassword");
            // --- FIM DA CORREÇÃO ---

            if (!ModelState.IsValid)
            {
                model.Security = new ChangePasswordViewModel();
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            user.NomeCompleto = model.Profile.NomeCompleto;
            user.Telefone = model.Profile.Telefone;
            user.Departamento = model.Profile.Departamento;
            user.Bio = model.Profile.Bio;

            if (model.Profile.ProfilePictureFile != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                if (!string.IsNullOrEmpty(user.Foto))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, user.Foto.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Profile.ProfilePictureFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Profile.ProfilePictureFile.CopyToAsync(fileStream);
                }
                user.Foto = "/uploads/avatars/" + uniqueFileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "Seu perfil foi atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            AddErrors(result);
            model.Security = new ChangePasswordViewModel();
            return View("Index", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(SettingsViewModel model)
        {
            TempData["ActiveTab"] = "Seguranca";

            // --- CORREÇÃO (BOA PRÁTICA) ---
            // Limpa a validação do formulário de perfil, pois ele não foi enviado
            ModelState.Remove("Profile.NomeCompleto");
            ModelState.Remove("Profile.Telefone");
            ModelState.Remove("Profile.Departamento");
            ModelState.Remove("Profile.Bio");
            ModelState.Remove("Profile.ProfilePictureFile");
            // --- FIM DA CORREÇÃO ---

            if (!ModelState.IsValid)
            {
                await RepopulateProfileDataAsync(model);
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.Security.OldPassword, model.Security.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                await RepopulateProfileDataAsync(model);
                return View("Index", model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Sua senha foi alterada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        #region Helpers

        private async Task RepopulateProfileDataAsync(SettingsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            model.Profile.NomeCompleto = user.NomeCompleto;
            model.Profile.Email = user.Email;
            model.Profile.Telefone = user.Telefone;
            model.Profile.Departamento = user.Departamento;
            model.Profile.Bio = user.Bio;
            model.Profile.ProfilePictureUrl = user.Foto;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion
    }
}