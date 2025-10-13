using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SolvITSupport.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> GetDashboardReportsAsync()
        {
            var ticketsByCategoryData = await _context.Tickets
                .Include(t => t.Categoria)
                .GroupBy(t => t.Categoria.Nome)
                .Select(g => new { Label = g.Key, Count = g.Count() })
                .ToListAsync();

            var ticketsByStatusData = await _context.Tickets
                .Include(t => t.Status)
                .GroupBy(t => t.Status.Nome)
                .Select(g => new { Label = g.Key, Count = g.Count() })
                .ToListAsync();

            var viewModel = new ReportViewModel
            {
                TicketsByCategory = new ChartData
                {
                    Labels = ticketsByCategoryData.Select(d => d.Label).ToList(),
                    Data = ticketsByCategoryData.Select(d => d.Count).ToList()
                },
                TicketsByStatus = new ChartData
                {
                    Labels = ticketsByStatusData.Select(d => d.Label).ToList(),
                    Data = ticketsByStatusData.Select(d => d.Count).ToList()
                }
            };

            return viewModel;
        }
    }
}