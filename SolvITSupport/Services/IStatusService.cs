using SolvITSupport.Models;

namespace SolvITSupport.Services
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetAllAsync();
    }
}