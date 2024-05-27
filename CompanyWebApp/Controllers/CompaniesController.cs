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
    public class CompaniesController : Controller
    {
        private readonly ItcompanyDbContext _context;

        public CompaniesController(ItcompanyDbContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            var companies = await _context.Companies
                                          .Include(c => c.Country)
                                          .ToListAsync();
            return View(companies);
        }

        // GET: Companies/Create
        public async Task<IActionResult> Create()
        {
            var countries = await _context.Countries
                                          .Select(c => new SelectListItem 
                                          { 
                                              Value = c.Id.ToString(), 
                                              Text = c.Name 
                                          })
                                          .ToListAsync();

            ViewData["CountryId"] = new SelectList(countries, "Value", "Text");
            return View();
        }

        // POST: Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City,Street,Header,StaffCount,CountryId,Website,Email,Edrpou")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var countries = await _context.Countries
                                          .Select(c => new SelectListItem 
                                          { 
                                              Value = c.Id.ToString(), 
                                              Text = c.Name 
                                          })
                                          .ToListAsync();
                                          
            ViewData["CountryId"] = new SelectList(countries, "Value", "Text", company.CountryId);
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var countries = await _context.Countries
                                          .Select(c => new SelectListItem 
                                          { 
                                              Value = c.Id.ToString(), 
                                              Text = c.Name 
                                          })
                                          .ToListAsync();
                                          
            ViewData["CountryId"] = new SelectList(countries, "Value", "Text", company.CountryId);
            return View(company);
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City,Street,Header,StaffCount,CountryId,Website,Email,Edrpou")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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

            var countries = await _context.Countries
                                          .Select(c => new SelectListItem 
                                          { 
                                              Value = c.Id.ToString(), 
                                              Text = c.Name 
                                          })
                                          .ToListAsync();
                                          
            ViewData["CountryId"] = new SelectList(countries, "Value", "Text", company.CountryId);
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                                        .Include(c => c.Country)
                                        .FirstOrDefaultAsync(m => m.Id == id);
                                        
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
