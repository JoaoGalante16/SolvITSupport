using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;       // <-- GARANTA QUE TEM ESTE 'using'
using SolvITSupport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolvITSupport.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IKnowledgeBaseService _kbService;

        public KnowledgeBaseController(ApplicationDbContext context, IKnowledgeBaseService kbService)
        {
            _context = context;
            _kbService = kbService;
        }

        // --- MÉTODO INDEX ATUALIZADO ---
        public async Task<IActionResult> Index(string searchString, string categoryName, int? pageNumber) // <-- 1. ADICIONE 'pageNumber'
        {
            // Define o tamanho da página
            int pageSize = 10; // Pode ajustar este valor

            var articlesQuery = _context.KnowledgeBaseArticles
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                articlesQuery = articlesQuery.Where(a => a.Titulo.Contains(searchString) || a.Descricao.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                articlesQuery = articlesQuery.Where(a => a.Categoria == categoryName);
            }

            // --- 2. REMOVA A LINHA ANTIGA ---
            // var articles = await articlesQuery.OrderByDescending(a => a.DataAtualizacao).ToListAsync();

            // --- 3. ADICIONE A CRIAÇÃO DA LISTA PAGINADA ---
            var paginatedArticles = await PaginatedList<KnowledgeBaseArticle>.CreateAsync(
                articlesQuery.OrderByDescending(a => a.DataAtualizacao).AsNoTracking(),
                pageNumber ?? 1,
                pageSize);


            // --- Lógica para os "Tópicos Populares" (continua igual) ---
            var popularTopics = await _context.KnowledgeBaseArticles
                .GroupBy(a => a.Categoria)
                .Select(g => new PopularTopicViewModel
                {
                    CategoryId = 0,
                    Name = g.Key,
                    ArticleCount = g.Count(),
                    Icon = GetCategoryIcon(g.Key)
                })
                .ToListAsync();

            // --- CÁLCULO REAL DAS ESTATÍSTICAS (continua igual) ---
            // Nota: Para performance, o ideal era fazer este cálculo apenas com base nos filtros,
            // mas para já vamos manter como estava, calculando sobre *todos* os artigos.
            var allArticles = await _context.KnowledgeBaseArticles.ToListAsync();
            int totalArticles = allArticles.Count;
            int totalViews = allArticles.Sum(a => a.Visualizacoes);
            int totalHelpfulVotes = allArticles.Sum(a => a.VotosUteis);

            double resolutionRate = (totalViews > 0)
                ? ((double)totalHelpfulVotes / totalViews) * 100
                : 0;

            var viewModel = new KnowledgeBaseViewModel
            {
                // --- 4. ATUALIZE AQUI ---
                Articles = paginatedArticles, // <-- Use a lista paginada
                // --- FIM DA MUDANÇA ---

                Categories = await _context.Categories.ToListAsync(),
                PopularTopics = popularTopics,
                CurrentSearchString = searchString,
                CurrentCategoryName = categoryName,

                TotalArticles = totalArticles,
                TotalViews = totalViews,
                ResolutionRate = Math.Round(resolutionRate, 0)
            };

            return View(viewModel);
        }

        // ... (O seu método Details(int? id) e GetCategoryIcon(string) continuam aqui) ...

        // GET: /KnowledgeBase/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var article = await _kbService.GetByIdAsync(id.Value);
            if (article == null)
            {
                return NotFound();
            }
            article.Visualizacoes++;
            _context.KnowledgeBaseArticles.Update(article);
            await _context.SaveChangesAsync();
            return View(article);
        }

        private static string GetCategoryIcon(string categoryName)
        {
            return categoryName?.ToLower() switch
            {
                "hardware" => "fa-wrench",
                "software" => "fa-laptop",
                "rede" => "fa-wifi",
                "acesso" => "fa-lock",
                "impressoras" => "fa-print",
                "e-mail" => "fa-envelope",
                _ => "fa-folder"
            };
        }
    }
}