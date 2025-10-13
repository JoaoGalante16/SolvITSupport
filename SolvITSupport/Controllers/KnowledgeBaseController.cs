using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolvITSupport.Services;
using System.Threading.Tasks;

namespace SolvITSupport.Controllers
{
    [Authorize] // Protege a base de conhecimento
    public class KnowledgeBaseController : Controller
    {
        private readonly IKnowledgeBaseService _kbService;

        public KnowledgeBaseController(IKnowledgeBaseService kbService)
        {
            _kbService = kbService;
        }

        // GET: /KnowledgeBase
        public async Task<IActionResult> Index()
        {
            var articles = await _kbService.GetAllAsync();
            return View(articles);
        }

        // GET: /KnowledgeBase/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var article = await _kbService.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }
    }
}