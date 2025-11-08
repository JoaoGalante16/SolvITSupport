using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SolvITSupport.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; // Precisamos disto para gerir os papéis
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // Adicione este método dentro da sua classe AdminController

        // GET: /Admin/Details/GUID_DO_UTILIZADOR
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Para podermos mostrar os papéis, precisamos de os ir buscar
            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);

            return View(user);
        }

        // --- INÍCIO DOS NOVOS MÉTODOS ---

        // GET: /Admin/Edit/GUID_DO_UTILIZADOR
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                NomeCompleto = user.NomeCompleto,
                Cargo = user.Cargo,
                Departamento = user.Departamento,
                UserRoles = userRoles,
                AllRoles = allRoles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList(),
                StatusMessage = TempData["StatusMessage"] as string
            };

            return View(model);
        }

        // POST: /Admin/Edit/GUID_DO_UTILIZADOR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Primeiro, verificamos a validação do modelo que veio do formulário
            if (ModelState.IsValid)
            {
                // Atualiza os dados do utilizador
                user.Email = model.Email;
                user.UserName = model.Email;
                user.NomeCompleto = model.NomeCompleto;
                user.Cargo = model.Cargo;
                user.Departamento = model.Departamento;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    // Atualiza os papéis (Roles)
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var selectedRoles = model.UserRoles ?? new string[] { };

                    await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
                    await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

                    return RedirectToAction(nameof(Users));
                }

                // Se o updateResult falhar, adiciona os erros ao ModelState
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // --- INÍCIO DA CORREÇÃO ---
            // Se chegámos aqui, significa que a validação falhou (ModelState.IsValid foi false)
            // ou o updateResult falhou. Precisamos de recarregar a lista de papéis
            // antes de mostrar o formulário novamente.
            var allRoles = await _roleManager.Roles.ToListAsync();
            model.AllRoles = allRoles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            // --- FIM DA CORREÇÃO ---

            return View(model);
        }

        // Adicione estes dois métodos dentro da classe AdminController

        // GET: /Admin/Delete/GUID_DO_UTILIZADOR
        // Mostra a página de confirmação antes de apagar.
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: /Admin/Delete/GUID_DO_UTILIZADOR
        // Executa a remoção após a confirmação.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Medida de segurança: não permitir que o admin se apague a si mesmo
                var currentUser = await _userManager.GetUserAsync(User);
                if (user.Id == currentUser.Id)
                {
                    // Poderíamos adicionar uma mensagem de erro aqui
                    return RedirectToAction(nameof(Users));
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Users));
                }
                // Se falhar, poderíamos mostrar os erros
            }

            return RedirectToAction(nameof(Users));
        }

        public IActionResult Create()
        {
            var model = new CreateUserViewModel
            {
                // Busca todos os perfis (Roles) e transforma-os numa lista para a dropdown
                AvailableRoles = _roleManager.Roles
                    .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                    .ToList()
            };
            return View(model);
        }

        // POST: Admin/Create
        // Processa o formulário preenchido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Cria o objeto ApplicationUser com os dados do modelo
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NomeCompleto = model.NomeCompleto,
                    CPF = model.CPF,
                    Cargo = model.Cargo
                    // Outros campos do ApplicationUser podem ser preenchidos aqui
                };

                // 2. Tenta criar o utilizador na base de dados com a password fornecida
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 3. Se a criação for bem-sucedida, atribui o perfil (Role) selecionado
                    await _userManager.AddToRoleAsync(user, model.RoleName);

                    TempData["StatusMessage"] = $"Utilizador '{user.Email}' criado com sucesso.";
                    return RedirectToAction(nameof(Users)); // Redireciona para a lista de utilizadores
                }

                // Se a criação falhar (ex: email já existe), adiciona os erros ao ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Se o ModelState for inválido (ou a criação falhar),
            // recarrega a lista de perfis e mostra o formulário novamente
            model.AvailableRoles = _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")] // Garante que só Admins podem fazer isso
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o usuário com ID '{id}'.");
            }

            // Defina sua senha padrão aqui.
            // ELA DEVE ser forte o suficiente para as regras do Identity
            string defaultPassword = "Senha123!";

            // Este é o método mais seguro:
            // 1. Gera um token de reset (como se o usuário tivesse pedido)
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // 2. Reseta a senha usando o token gerado
            var result = await _userManager.ResetPasswordAsync(user, token, defaultPassword);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Admin resetou a senha do usuário {user.UserName}.");
                // Envia a mensagem de sucesso de volta para a página Edit
                TempData["StatusMessage"] = $"Senha do usuário {user.UserName} resetada para: {defaultPassword}";
            }
            else
            {
                // Coleta os erros, caso haja
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["StatusMessage"] = $"Erro ao resetar senha: {errors}";
            }

            // Redireciona de volta para a página de Edição do mesmo usuário
            return RedirectToAction("Edit", new { id = user.Id });
        }
    }
}