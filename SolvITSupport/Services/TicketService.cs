using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolvITSupport.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public TicketService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridade)
                .Include(t => t.Status)
                .Include(t => t.Solicitante)
                .Include(t => t.Atendente)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridade)
                .Include(t => t.Status)
                .Include(t => t.Solicitante)
                .Include(t => t.Atendente)
                .Include(t => t.Atualizacoes)
                    .ThenInclude(u => u.Usuario)
                .Include(t => t.Anexos) // <-- ADICIONE ESTA LINHA
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(string userId)
        {
            return await _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridade)
                .Include(t => t.Status)
                .Where(t => t.SolicitanteId == userId)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            ticket.DataAtualizacao = System.DateTime.Now;
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddUpdateAsync(int ticketId, string userId, string content)
        {
            var ticket = await _context.Tickets
                                    .Include(t => t.Atualizacoes) // Incluímos as atualizações
                                    .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket != null)
            {
                var update = new TicketUpdate
                {
                    UsuarioId = userId,
                    Conteudo = content,
                    DataCriacao = DateTime.Now
                };

                // Adicionamos a nova atualização diretamente à lista do ticket
                ticket.Atualizacoes.Add(update);
                ticket.DataAtualizacao = DateTime.Now;

                // Salvamos as alterações no contexto
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangeStatusAsync(int ticketId, string userId, int newStatusId)
        {
            var ticket = await _context.Tickets
                                    .Include(t => t.Status)
                                    .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket != null && ticket.StatusId != newStatusId)
            {
                var newStatus = await _context.Statuses.AsNoTracking().FirstOrDefaultAsync(s => s.Id == newStatusId);
                if (newStatus == null) return;

                string oldStatusName = ticket.Status?.Nome ?? "N/A";

                // 1. Atualiza o ticket
                ticket.StatusId = newStatusId;
                ticket.DataAtualizacao = DateTime.Now;

                // 2. Adiciona o log
                var update = new TicketUpdate
                {
                    UsuarioId = userId,
                    Conteudo = $"Status alterado de '{oldStatusName}' para '{newStatus.Nome}'.",
                    DataCriacao = DateTime.Now
                };
                ticket.Atualizacoes.Add(update); // Assume que Ticket.Atualizacoes é carregado ou inicializado

                // 3. Salva as alterações
                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangePriorityAsync(int ticketId, string userId, int newPriorityId)
        {
            var ticket = await _context.Tickets
                                    .Include(t => t.Prioridade)
                                    .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket != null && ticket.PrioridadeId != newPriorityId)
            {
                var newPriority = await _context.Priorities.AsNoTracking().FirstOrDefaultAsync(p => p.Id == newPriorityId);
                if (newPriority == null) return;

                string oldPriorityName = ticket.Prioridade?.Nome ?? "N/A";

                // 1. Atualiza o ticket
                ticket.PrioridadeId = newPriorityId;
                ticket.DataAtualizacao = DateTime.Now;

                // 2. Adiciona o log
                var update = new TicketUpdate
                {
                    UsuarioId = userId,
                    Conteudo = $"Prioridade alterada de '{oldPriorityName}' para '{newPriority.Nome}'.",
                    DataCriacao = DateTime.Now
                };
                ticket.Atualizacoes.Add(update);

                // 3. Salva as alterações
                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangeAssigneeAsync(int ticketId, string actingUserId, string newAssigneeId)
        {
            var ticket = await _context.Tickets
                                    .Include(t => t.Atendente) // O Atendente é um ApplicationUser
                                    .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket != null && ticket.AtendenteId != newAssigneeId)
            {
                // Busca o NOVO Atendente para o log
                var newAssignee = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == newAssigneeId);
                // Permite atribuir a 'null' (desatribuir)
                if (newAssigneeId != null && newAssignee == null) return;

                string oldAssigneeName = ticket.Atendente?.NomeCompleto ?? "Ninguém";
                string newAssigneeName = newAssignee?.NomeCompleto ?? "Ninguém";

                // 1. Atualiza o ticket
                ticket.AtendenteId = newAssigneeId;
                ticket.DataAtualizacao = DateTime.Now;

                // 2. Adiciona o log
                var update = new TicketUpdate
                {
                    UsuarioId = actingUserId, // Usuário logado que está fazendo a mudança
                    Conteudo = $"Atribuído de '{oldAssigneeName}' para '{newAssigneeName}'.",
                    DataCriacao = DateTime.Now
                };
                ticket.Atualizacoes.Add(update);

                // 3. Salva as alterações
                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

    }
}