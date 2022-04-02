#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo.Areas.Identity.Data;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class AmbientesController : Controller
    {
        private readonly Context _context;
        private readonly HttpContext _http;

        public AmbientesController(Context context, IHttpContextAccessor accessor)
        {
            _context = context;
            _http = accessor.HttpContext ;
        }

        // GET: Ambientes
        public async Task<IActionResult> Index()
        {

            _http.Request.Headers.TryGetValue("user-id", out var userId) ;
            

            var context = _context.Ambientes.Include(a => a.User).Where(m => m.UserId == userId.ToString());
            return View(await context.ToListAsync());
        }

        // GET: Ambientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambiente = await _context.Ambientes
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ambiente == null)
            {
                return NotFound();
            }

            return View(ambiente);
        }

        // GET: Ambientes/Create
        public IActionResult Create()
        {

            _http.Request.Headers.TryGetValue("user-id", out var userId) ;
            ViewData["AuthId"] = userId.ToString() ;

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Ambientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,UserId")] Ambiente ambiente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ambiente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ambiente.UserId);
            return View(ambiente);
        }

        // GET: Ambientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambiente = await _context.Ambientes.FindAsync(id);
            if (ambiente == null)
            {
                return NotFound();
            }

            _http.Request.Headers.TryGetValue("user-id", out var userId) ;
            ViewData["AuthId"] = userId.ToString() ;

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ambiente.UserId);
            return View(ambiente);
        }

        // POST: Ambientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,UserId")] Ambiente ambiente)
        {
            if (id != ambiente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ambiente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmbienteExists(ambiente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ambiente.UserId);
            return View(ambiente);
        }

        // GET: Ambientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambiente = await _context.Ambientes
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ambiente == null)
            {
                return NotFound();
            }

            return View(ambiente);
        }

        // POST: Ambientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ambiente = await _context.Ambientes.FindAsync(id);
            _context.Ambientes.Remove(ambiente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmbienteExists(int id)
        {
            return _context.Ambientes.Any(e => e.Id == id);
        }
    }
}
