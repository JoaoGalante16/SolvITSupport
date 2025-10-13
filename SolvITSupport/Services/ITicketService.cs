using SolvITSupport.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolvITSupport.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task<Ticket> CreateTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task<IEnumerable<Ticket>> GetTicketsByUserAsync(string userId);
        Task AddUpdateAsync(int ticketId, string userId, string content);
        Task AssignTicketAsync(int ticketId, string attendantId, string assignerId);
        Task ChangeTicketStatusAsync(int ticketId, int newStatusId, string userId);
        Task ChangeTicketPriorityAsync(int ticketId, int newPriorityId, string userId);
    }
}