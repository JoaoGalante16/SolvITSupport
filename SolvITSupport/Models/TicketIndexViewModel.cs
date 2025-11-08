using Microsoft.AspNetCore.Mvc.Rendering;
using SolvITSupport.Models;

namespace SolvITSupport.Models
{
    public class TicketIndexViewModel
    {
        // A lista de chamados já filtrada
        public PaginatedList<Ticket> Tickets { get; set; }

        // Opções para os menus dropdown de filtro
        public SelectList Categories { get; set; }
        public SelectList Priorities { get; set; }
        public SelectList Statuses { get; set; }

        // Os valores selecionados nos filtros
        public int? CategoryIdFilter { get; set; }
        public int? PriorityIdFilter { get; set; }
        public int? StatusIdFilter { get; set; }

        // O texto da pesquisa
        public string SearchString { get; set; }

        public int TotalCount { get; set; }
        public int OpenCount { get; set; }
        public int InProgressCount { get; set; }
        public int ResolvedCount { get; set; }
    }
}