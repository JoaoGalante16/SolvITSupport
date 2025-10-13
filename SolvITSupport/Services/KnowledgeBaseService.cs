using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolvITSupport.Services
{
    public class KnowledgeBaseService : IKnowledgeBaseService
    {
        private readonly ApplicationDbContext _context;

        public KnowledgeBaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KnowledgeBaseArticle>> GetAllAsync()
        {
            return await _context.KnowledgeBaseArticles.Where(a => a.Publicado).ToListAsync();
        }

        public async Task<KnowledgeBaseArticle?> GetByIdAsync(int id)
        {
            return await _context.KnowledgeBaseArticles.FindAsync(id);
        }
    }
}