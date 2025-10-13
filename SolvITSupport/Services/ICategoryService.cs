using SolvITSupport.Models;

namespace SolvITSupport.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
    }
}