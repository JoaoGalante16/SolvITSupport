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
    }
}