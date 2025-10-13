using Microsoft.AspNetCore.Http; // Necessário para IFormFile
using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    public class CreateTicketViewModel
    {
        [Required(ErrorMessage = "O Título é obrigatório")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Descrição é obrigatória")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecione uma Categoria")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Selecione uma Prioridade")]
        [Display(Name = "Prioridade")]
        public int PrioridadeId { get; set; }

        // ADICIONE ESTA PROPRIEDADE
        [Display(Name = "Anexo")]
        public IFormFile? Anexo { get; set; } // O '?' indica que é opcional
    }
}