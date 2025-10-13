using System.Collections.Generic;

namespace SolvITSupport.Models
{
    // Guarda os dados para um gráfico de barras ou linhas
    public class ChartData
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<int> Data { get; set; } = new List<int>();
    }

    // Modelo principal para a página de relatórios
    public class ReportViewModel
    {
        public ChartData TicketsByCategory { get; set; }
        public ChartData TicketsByStatus { get; set; }
    }
}