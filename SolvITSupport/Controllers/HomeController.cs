using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // <-- Adicione esta linha, se não existir
using Microsoft.AspNetCore.Mvc;
using SolvITSupport.Models;
using SolvITSupport.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SolvITSupport.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager; // Adicione esta linha

        // O seu construtor já está correto
        public HomeController(ITicketService ticketService, UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }

        // Substitua o seu método Index() por este
        // Substitua o seu método Index() por este
        public async Task<IActionResult> Index(string filter = "Todos")
        {
            // Primeiro, obtemos a lista completa de chamados com base nas permissões
            IEnumerable<Ticket> allTickets;
            var currentUserId = _userManager.GetUserId(User);

            if (User.IsInRole("Atendente") || User.IsInRole("Administrador"))
            {
                allTickets = await _ticketService.GetAllTicketsAsync();
            }
            else
            {
                allTickets = await _ticketService.GetTicketsByUserAsync(currentUserId);
            }

            // --- INÍCIO DA LÓGICA DE FILTRO ---
            IEnumerable<Ticket> filteredTickets = allTickets;

            switch (filter)
            {
                case "Aberto":
                    filteredTickets = allTickets.Where(t => t.Status.Nome == "Aberto");
                    break;
                case "Em Andamento":
                    filteredTickets = allTickets.Where(t => t.Status.Nome == "Em Andamento");
                    break;
                case "Resolvidos":
                    filteredTickets = allTickets.Where(t => t.Status.Nome == "Resolvido");
                    break;
                default: // "Todos"
                    filteredTickets = allTickets;
                    break;
            }
            // --- FIM DA LÓGICA DE FILTRO ---

            // Preenchemos o ViewModel com as estatísticas (usando allTickets) E a lista filtrada
            var viewModel = new DashboardViewModel
            {
                TotalTickets = allTickets.Count(),
                OpenTickets = allTickets.Count(t => t.Status.Nome == "Aberto"),
                InProgressTickets = allTickets.Count(t => t.Status.Nome == "Em Andamento"),
                ResolvedTickets = allTickets.Count(t => t.Status.Nome == "Resolvido"),
                RecentTickets = filteredTickets.OrderByDescending(t => t.DataCriacao).Take(10)
            };

            // Guardamos o filtro atual para que a View saiba qual aba destacar
            ViewBag.CurrentFilter = filter;

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}