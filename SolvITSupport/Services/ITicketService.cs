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
        Task ChangeStatusAsync(int ticketId, string userId, int newStatusId);
        Task ChangePriorityAsync(int ticketId, string userId, int newPriorityId);
        Task ChangeAssigneeAsync(int ticketId, string actingUserId, string newAssigneeId);

    }
}