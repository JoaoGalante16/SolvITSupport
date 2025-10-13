using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Models;

namespace SolvITSupport.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TicketUpdate> TicketUpdates { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<KnowledgeBaseArticle> KnowledgeBaseArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Mapeamento explícito dos nomes das tabelas (A SOLUÇÃO ESTÁ AQUI)
            builder.Entity<ApplicationUser>().ToTable("Users"); // Garante que a tabela de utilizadores se chama "Users"
            builder.Entity<Ticket>().ToTable("Tickets");
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Priority>().ToTable("Priorities");
            builder.Entity<Status>().ToTable("Statuses");
            builder.Entity<TicketUpdate>().ToTable("TicketUpdates");
            builder.Entity<Attachment>().ToTable("Attachments");
            builder.Entity<Notification>().ToTable("Notifications");

            // --- O resto do código continua igual ---

            // Configurar relacionamentos
            builder.Entity<Ticket>()
                .HasOne(t => t.Solicitante)
                .WithMany(u => u.ChamadosSolicitados)
                .HasForeignKey(t => t.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.Atendente)
                .WithMany(u => u.ChamadosAtendidos)
                .HasForeignKey(t => t.AtendenteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para performance
            builder.Entity<Ticket>()
                .HasIndex(t => t.StatusId);

            builder.Entity<Ticket>()
                .HasIndex(t => t.SolicitanteId);

            builder.Entity<Ticket>()
                .HasIndex(t => t.AtendenteId);

            builder.Entity<Ticket>()
                .HasIndex(t => t.DataCriacao);

            builder.Entity<Notification>()
                .HasIndex(n => new { n.UsuarioId, n.Lida });
        }
    }
}
