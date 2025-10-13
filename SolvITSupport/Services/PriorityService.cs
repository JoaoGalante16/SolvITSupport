using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;

namespace SolvITSupport.Services
{
    public class PriorityService : IPriorityService
    {
        private readonly ApplicationDbContext _context;

        public PriorityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Priority>> GetAllAsync()
        {
            return await _context.Priorities.ToListAsync();
        }
    }
}