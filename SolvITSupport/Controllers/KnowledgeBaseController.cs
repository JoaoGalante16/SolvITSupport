using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolvITSupport.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnowledgeBaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string categoryName) // MUDANÇA AQUI
        {
            var articlesQuery = _context.KnowledgeBaseArticles
                                        //.Include(a => a.Categoria) // Não podemos mais fazer Include
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                articlesQuery = articlesQuery.Where(a => a.Titulo.Contains(searchString) || a.Descricao.Contains(searchString));
            }

            // --- Lógica de filtro de Categoria (MUDOU) ---
            if (!string.IsNullOrEmpty(categoryName))
            {
                articlesQuery = articlesQuery.Where(a => a.Categoria == categoryName);
            }

            var articles = await articlesQuery.OrderByDescending(a => a.DataAtualizacao).ToListAsync();

            // --- Lógica para os "Tópicos Populares" (MUDOU) ---
            var popularTopics = await _context.KnowledgeBaseArticles
                .GroupBy(a => a.Categoria) // Agrupa pelo NOME (string)
                .Select(g => new PopularTopicViewModel
                {
                    CategoryId = 0, // Já não temos um ID de categoria aqui
                    Name = g.Key,   // g.Key é o NOME da categoria
                    ArticleCount = g.Count(),
                    Icon = GetCategoryIcon(g.Key)
                })
                .ToListAsync();

            // --- CÁLCULO REAL DAS ESTATÍSTICAS ---
            var allArticles = await _context.KnowledgeBaseArticles.ToListAsync();
            int totalArticles = allArticles.Count;
            int totalViews = allArticles.Sum(a => a.Visualizacoes);
            int totalHelpfulVotes = allArticles.Sum(a => a.VotosUteis);

            double resolutionRate = (totalViews > 0)
                ? ((double)totalHelpfulVotes / totalViews) * 100
                : 0;

            var viewModel = new KnowledgeBaseViewModel
            {
                Articles = articles,
                Categories = await _context.Categories.ToListAsync(),
                PopularTopics = popularTopics,
                CurrentSearchString = searchString,
                CurrentCategoryName = categoryName, // MUDANÇA AQUI

                TotalArticles = totalArticles,
                TotalViews = totalViews,
                ResolutionRate = Math.Round(resolutionRate, 0)
            };

            return View(viewModel);
        }

        // Método auxiliar simples para mapear nomes de categorias para ícones FontAwesome
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
                _ => "fa-folder" // Ícone padrão
            };
        }
    }
}