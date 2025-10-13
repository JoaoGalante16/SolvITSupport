using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    public class ChangePriorityViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "É necessário selecionar uma prioridade.")]
        [Display(Name = "Nova Prioridade")]
        public int NewPriorityId { get; set; }

        public SelectList Priorities { get; set; }
    }
}