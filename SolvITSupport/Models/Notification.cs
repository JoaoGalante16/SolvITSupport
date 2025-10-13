using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolvITSupport.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;

        public int? TicketId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Mensagem { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = "Informacao";

        public bool Lida { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataLeitura { get; set; }

        // Relacionamentos
        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser? Usuario { get; set; }

        [ForeignKey("TicketId")]
        public virtual Ticket? Ticket { get; set; }
    }
}
