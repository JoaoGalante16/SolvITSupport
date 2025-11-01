using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolvITSupport.Services;
using System.Threading.Tasks;
using System.Text;

namespace SolvITSupport.Controllers
{
    [Authorize(Roles = "Administrador, Atendente")] // Apenas Admins e Atendentes podem ver os relatórios
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: /Reports
        public async Task<IActionResult> Index()
        {
            var viewModel = await _reportService.GetDashboardReportsAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Export()
        {
            // 1. Chama o serviço para gerar o conteúdo do CSV
            var csvData = await _reportService.GenerateTicketsCsvAsync();

            // 2. Define o nome do ficheiro
            var fileName = $"Relatorio_Chamados_{DateTime.Now:yyyyMMdd_HHmm}.csv";

            // 3. Retorna o ficheiro para o utilizador
            // Usamos Encoding.UTF8 para garantir compatibilidade com acentos (ç, ã, etc.)
            return File(Encoding.UTF8.GetBytes(csvData), "text/csv", fileName);
        }
    }
}