using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolvITSupport.Services;
using System.Threading.Tasks;

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
    }
}