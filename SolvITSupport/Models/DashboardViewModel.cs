using System.Collections.Generic;

namespace SolvITSupport.Models
{
    public class DashboardViewModel
    {
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int InProgressTickets { get; set; }
        public int ResolvedTickets { get; set; }

        // ADICIONE ESTA LINHA
        public IEnumerable<Ticket> RecentTickets { get; set; }
    }
}