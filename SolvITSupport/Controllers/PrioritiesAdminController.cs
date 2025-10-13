using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SolvITSupport.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PrioritiesAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrioritiesAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /PrioritiesAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Priorities.ToListAsync());
        }

        // GET: /PrioritiesAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var priority = await _context.Priorities.FirstOrDefaultAsync(m => m.Id == id);
            if (priority == null) return NotFound();
            return View(priority);
        }

        // GET: /PrioritiesAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /PrioritiesAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Priority priority)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priority);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(priority);
        }

        // GET: /PrioritiesAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var priority = await _context.Priorities.FindAsync(id);
            if (priority == null) return NotFound();
            return View(priority);
        }

        // POST: /PrioritiesAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Priority priority)
        {
            if (id != priority.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(priority);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(priority);
        }

        // GET: /PrioritiesAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var priority = await _context.Priorities.FirstOrDefaultAsync(m => m.Id == id);
            if (priority == null) return NotFound();
            return View(priority);
        }

        // POST: /PrioritiesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);
            if (priority != null)
            {
                _context.Priorities.Remove(priority);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}