using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    public class ChangeStatusViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "É necessário selecionar um status.")]
        [Display(Name = "Novo Status")]
        public int NewStatusId { get; set; }

        public SelectList Statuses { get; set; }
    }
}