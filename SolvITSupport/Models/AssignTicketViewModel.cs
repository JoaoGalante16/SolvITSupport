using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    public class AssignTicketViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "É necessário selecionar um atendente.")]
        [Display(Name = "Atribuir para")]
        public string AttendantId { get; set; }

        public SelectList Attendants { get; set; }
    }
}