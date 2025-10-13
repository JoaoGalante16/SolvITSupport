using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;

namespace SolvITSupport.Services
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext _context;

        public StatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _context.Statuses.ToListAsync();
        }
    }
}