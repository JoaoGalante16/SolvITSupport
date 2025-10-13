using System.ComponentModel.DataAnnotations;

namespace SolvITSupport.Models
{
    public class Priority
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Tempo Resposta SLA (horas)")]
        public int TempoRespostaSLA { get; set; }

        [Required]
        [Display(Name = "Tempo Resolução SLA (horas)")]
        public int TempoResolucaoSLA { get; set; }

        [StringLength(20)]
        [Display(Name = "Cor")]
        public string? Cor { get; set; }

        [Required]
        [Display(Name = "Nível")]
        public int Nivel { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // Relacionamentos
        public virtual ICollection<Ticket> Chamados { get; set; } = new List<Ticket>();
    }
}
