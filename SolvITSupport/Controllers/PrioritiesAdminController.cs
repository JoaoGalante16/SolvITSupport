using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
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

        public async Task<IActionResult> Index() => View(await _context.Priorities.ToListAsync());
        public async Task<IActionResult> Details(int? id) => View(await _context.Priorities.FindAsync(id));
        public IActionResult Create() => View();
        public async Task<IActionResult> Edit(int? id) => View(await _context.Priorities.FindAsync(id));
        public async Task<IActionResult> Delete(int? id) => View(await _context.Priorities.FindAsync(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Priority priority)
        {
            _context.Add(priority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Priority priority)
        {
            _context.Update(priority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);
            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}