using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;

namespace SolvITSupport.Controllers
{
    // Este Controller não terá uma página visual (View),
    // ele apenas executa uma ação e devolve uma mensagem de texto.
    public class SeedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Este método será executado quando acedermos a /Seed/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                // Apagamos o banco de dados para garantir um teste limpo
                await _context.Database.EnsureDeletedAsync();

                // Mandamos criar o banco novamente com base nas migrations
                await _context.Database.MigrateAsync();

                // Chamamos o nosso inicializador de dados
                await DbInitializer.Initialize(_context, _userManager, _roleManager);

                // Se tudo correu bem, devolvemos uma mensagem de sucesso
                return Content("Banco de dados recriado e populado com sucesso!");
            }
            catch (Exception ex)
            {
                // Se algo falhar, devolvemos a mensagem de erro detalhada
                return Content($"Ocorreu um erro: {ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }
}