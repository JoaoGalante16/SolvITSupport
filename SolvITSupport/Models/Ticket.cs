using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolvITSupport.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Required]
        [Display(Name = "Prioridade")]
        public int PrioridadeId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Required]
        public string SolicitanteId { get; set; } = string.Empty;

        [Display(Name = "Atendente")]
        public string? AtendenteId { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Última Atualização")]
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        [Display(Name = "Data de Atribuição")]
        public DateTime? DataAtribuicao { get; set; }

        [Display(Name = "Data da Primeira Resposta")]
        public DateTime? DataPrimeiraResposta { get; set; }

        [Display(Name = "Data de Resolução")]
        public DateTime? DataResolucao { get; set; }

        [Display(Name = "Data de Fechamento")]
        public DateTime? DataFechamento { get; set; }

        [Display(Name = "Prazo SLA")]
        public DateTime? PrazoSLA { get; set; }

        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Avaliação")]
        [Range(1, 5, ErrorMessage = "A avaliação deve ser entre 1 e 5")]
        public int? AvaliacaoNota { get; set; }

        [Display(Name = "Comentário da Avaliação")]
        [StringLength(500)]
        public string? AvaliacaoComentario { get; set; }

        [Display(Name = "SLA Atendido")]
        public bool SLAAtendido { get; set; } = true;

        [NotMapped]
        public string Codigo => $"TK-{Id:D4}";

        // Relacionamentos
        [ForeignKey("CategoriaId")]
        public virtual Category? Categoria { get; set; }

        [ForeignKey("PrioridadeId")]
        public virtual Priority? Prioridade { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status? Status { get; set; }

        [ForeignKey("SolicitanteId")]
        public virtual ApplicationUser? Solicitante { get; set; }

        [ForeignKey("AtendenteId")]
        public virtual ApplicationUser? Atendente { get; set; }

        public virtual ICollection<TicketUpdate> Atualizacoes { get; set; } = new List<TicketUpdate>();
        public virtual ICollection<Attachment> Anexos { get; set; } = new List<Attachment>();
    }
}
