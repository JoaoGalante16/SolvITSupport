using SolvITSupport.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolvITSupport.Services
{
    public interface IKnowledgeBaseService
    {
        Task<IEnumerable<KnowledgeBaseArticle>> GetAllAsync();
        Task<KnowledgeBaseArticle?> GetByIdAsync(int id);
    }
}