using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SolvITSupport.Models
{
    public class TicketIndexViewModel
    {
        // A lista de chamados já filtrada
        public IEnumerable<Ticket> Tickets { get; set; }

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
    }
}