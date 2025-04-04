using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class GstTaxesController : Controller
    {
        private readonly WebMobiTask1DbContext _context;

        public GstTaxesController(WebMobiTask1DbContext context)
        {
            _context = context;
        }

        // GET: Admin/GstTaxes
        public async Task<IActionResult> Index()
        {
            var webMobiTask1DbContext = _context.GstTax.Include(g => g.Subcategory).ThenInclude(c=>c.Category);
            return View(await webMobiTask1DbContext.ToListAsync());
        }

        // GET: Admin/GstTaxes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gstTax = await _context.GstTax
                .Include(g => g.Subcategory)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(m => m.GSTId == id);
            if (gstTax == null)
            {
                return NotFound();
            }

            return View(gstTax);
        }

        // GET: Admin/GstTaxes/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            ViewBag.Subcategories = new SelectList(new List<Subcategory>(), "SubcategoryId", "Name");

            return View();
        }
        [HttpGet]
        public JsonResult GetSubcategories(int categoryId)
        {
            var subcategories = _context.Subcategories
                .Where(s => s.CategoryId == categoryId)
                .Select(s => new { s.SubcategoryId, s.Name })
                .ToList();

            return Json(subcategories);
        }

        // POST: Admin/GstTaxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GSTId,SubcategoryId,CGST,SGST,CreatedAt,UpdatedAt")] GstTax gstTax)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gstTax);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SubcategoryId = new SelectList(_context.Subcategories, "SubcategoryId", "Name", gstTax.SubcategoryId);
            
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View(gstTax);
        }

        // GET: Admin/GstTaxes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gstTax = await _context.GstTax.FindAsync(id);
            if (gstTax == null)
            {
                return NotFound();
            }
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name", gstTax.SubcategoryId);
            return View(gstTax);
        }

        // POST: Admin/GstTaxes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GSTId,SubcategoryId,CGST,SGST,CreatedAt,UpdatedAt")] GstTax gstTax)
        {
            if (id != gstTax.GSTId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gstTax);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GstTaxExists(gstTax.GSTId))
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
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name", gstTax.SubcategoryId);
            return View(gstTax);
        }

        // GET: Admin/GstTaxes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gstTax = await _context.GstTax
                .Include(g => g.Subcategory)
                .ThenInclude(c=>c.Category)
                .FirstOrDefaultAsync(m => m.GSTId == id);
            if (gstTax == null)
            {
                return NotFound();
            }

            return View(gstTax);
        }

        // POST: Admin/GstTaxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gstTax = await _context.GstTax.FindAsync(id);
            if (gstTax != null)
            {
                _context.GstTax.Remove(gstTax);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GstTaxExists(int id)
        {
            return _context.GstTax.Any(e => e.GSTId == id);
        }
    }
}
