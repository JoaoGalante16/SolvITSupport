using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolvITSupport.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required]
        [StringLength(255)]
        public string NomeArquivo { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string NomeOriginal { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Caminho { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TipoArquivo { get; set; }

        public long? TamanhoBytes { get; set; }

        public DateTime DataUpload { get; set; } = DateTime.Now;

        [Required]
        public string UsuarioId { get; set; } = string.Empty;

        // Relacionamentos
        [ForeignKey("TicketId")]
        public virtual Ticket? Ticket { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser? Usuario { get; set; }
    }
}
