using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using System.Threading.Tasks;

namespace SolvITSupport.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class StatusAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Statuses.ToListAsync());
        public async Task<IActionResult> Details(int? id) => View(await _context.Statuses.FindAsync(id));
        public IActionResult Create() => View();
        public async Task<IActionResult> Edit(int? id) => View(await _context.Statuses.FindAsync(id));
        public async Task<IActionResult> Delete(int? id) => View(await _context.Statuses.FindAsync(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Status status)
        {
            _context.Add(status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Status status)
        {
            _context.Update(status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}