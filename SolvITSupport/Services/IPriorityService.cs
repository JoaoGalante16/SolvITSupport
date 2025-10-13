using SolvITSupport.Models;

namespace SolvITSupport.Services
{
    public interface IPriorityService
    {
        Task<IEnumerable<Priority>> GetAllAsync();
    }
}