using Microsoft.AspNetCore.Identity;

namespace SolvITSupport.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string? CPF { get; set; }
        public string? Telefone { get; set; }
        public string? Cargo { get; set; }
        public string? Departamento { get; set; }
        public string? Foto { get; set; }
        public string? Bio { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? UltimoAcesso { get; set; }
        public bool Ativo { get; set; } = true;

        // Relacionamentos
        public virtual ICollection<Ticket> ChamadosSolicitados { get; set; } = new List<Ticket>();
        public virtual ICollection<Ticket> ChamadosAtendidos { get; set; } = new List<Ticket>();
        public virtual ICollection<TicketUpdate> Atualizacoes { get; set; } = new List<TicketUpdate>();
        public virtual ICollection<Notification> Notificacoes { get; set; } = new List<Notification>();
    }
}
