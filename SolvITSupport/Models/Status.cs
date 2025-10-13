using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [StringLength(20)]
        [Display(Name = "Cor")]
        public string? Cor { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = string.Empty;

        [Display(Name = "Ordem")]
        public int Ordem { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // Relacionamentos
        public virtual ICollection<Ticket> Chamados { get; set; } = new List<Ticket>();
    }
}
