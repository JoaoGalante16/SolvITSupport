using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    public class TicketDetailsViewModel
    {
        public Ticket Ticket { get; set; }
        public IEnumerable<TicketUpdate> Updates { get; set; }

        [Display(Name = "Novo Comentário")]
        [Required(ErrorMessage = "O comentário não pode estar vazio.")]
        public string NewComment { get; set; }
    }
}