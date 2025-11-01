using SolvITSupport.Models;
using System.Threading.Tasks;

namespace SolvITSupport.Services
{
    public interface IReportService
    {
        Task<ReportViewModel> GetDashboardReportsAsync();
        Task<string> GenerateTicketsCsvAsync();
    }
}