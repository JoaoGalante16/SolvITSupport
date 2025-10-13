using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolvITSupport.Models
{
    public class TicketUpdate
    {
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;

        [Required]
        public string Conteudo { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = "Comentario";

        public bool Publico { get; set; } = true;

        // Relacionamentos
        [ForeignKey("TicketId")]
        public virtual Ticket? Ticket { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser? Usuario { get; set; }
    }
}
