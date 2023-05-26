using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompanyWebApp.Models;

namespace CompanyWebApp.Controllers
{
    public class ProgrammersController : Controller
    {
        private readonly ItcompanyDbContext _context;

        public ProgrammersController(ItcompanyDbContext context)
        {
            _context = context;
        }

        // GET: Programmers
        public async Task<IActionResult> Index()
        {
            var itcompanyDbContext = _context.Programmers.Include(p => p.Citizen).Include(p => p.Company);
            return View(await itcompanyDbContext.ToListAsync());
        }

        // GET: Programmers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Programmers == null)
            {
                return NotFound();
            }

            var programmer = await _context.Programmers
                .Include(p => p.Citizen)
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programmer == null)
            {
                return NotFound();
            }

            return View(programmer);
        }

        // GET: Programmers/Create
        public IActionResult Create()
        {
            ViewData["CitizenId"] = new SelectList(_context.Citizens, "Id", "Id");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id");
            return View();
        }

        // POST: Programmers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Specialization,CitizenId,CompanyId,YearsExperience,Range,Language,Salary,Time,Place")] Programmer programmer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programmer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CitizenId"] = new SelectList(_context.Citizens, "Id", "Id", programmer.CitizenId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", programmer.CompanyId);
            return View(programmer);
        }

        // GET: Programmers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Programmers == null)
            {
                return NotFound();
            }

            var programmer = await _context.Programmers.FindAsync(id);
            if (programmer == null)
            {
                return NotFound();
            }
            ViewData["CitizenId"] = new SelectList(_context.Citizens, "Id", "Id", programmer.CitizenId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", programmer.CompanyId);
            return View(programmer);
        }

        // POST: Programmers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Specialization,CitizenId,CompanyId,YearsExperience,Range,Language,Salary,Time,Place")] Programmer programmer)
        {
            if (id != programmer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programmer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgrammerExists(programmer.Id))
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
            ViewData["CitizenId"] = new SelectList(_context.Citizens, "Id", "Id", programmer.CitizenId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", programmer.CompanyId);
            return View(programmer);
        }

        // GET: Programmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Programmers == null)
            {
                return NotFound();
            }

            var programmer = await _context.Programmers
                .Include(p => p.Citizen)
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programmer == null)
            {
                return NotFound();
            }

            return View(programmer);
        }

        // POST: Programmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Programmers == null)
            {
                return Problem("Entity set 'ItcompanyDbContext.Programmers'  is null.");
            }
            var programmer = await _context.Programmers.FindAsync(id);
            if (programmer != null)
            {
                _context.Programmers.Remove(programmer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgrammerExists(int id)
        {
          return _context.Programmers.Any(e => e.Id == id);
        }
    }
}
