using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    // Este ViewModel representa os campos que podem ser editados no nosso formulário.
    public class EditTicketViewModel
    {
        public int Id { get; set; } // Precisamos do ID para saber qual ticket atualizar

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

        [Required(ErrorMessage = "Selecione um Status")]
        [Display(Name = "Status")]
        public int StatusId { get; set; }
    }
}