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
    public class VariantAttributeValuesController : Controller
    {
        private readonly WebMobiTask1DbContext _context;

        public VariantAttributeValuesController(WebMobiTask1DbContext context)
        {
            _context = context;
        }

        // GET: Admin/VariantAttributeValues
        public async Task<IActionResult> Index()
        {
            var webMobiTask1DbContext = _context.VariantAttributeValues.Include(v => v.ProductAttributeValue).Include(v => v.ProductVariant);
            return View(await webMobiTask1DbContext.ToListAsync());
        }

        // GET: Admin/VariantAttributeValues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variantAttributeValue = await _context.VariantAttributeValues
                .Include(v => v.ProductAttributeValue)
                .Include(v => v.ProductVariant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variantAttributeValue == null)
            {
                return NotFound();
            }

            return View(variantAttributeValue);
        }

        // GET: Admin/VariantAttributeValues/Create
        public IActionResult Create()
        {
            ViewData["AttrbuteValueId"] = new SelectList(_context.ProductAttributeValues, "ValueId", "Value");
            ViewData["VarinatId"] = new SelectList(_context.ProductVariants, "VariantId", "SKU");
            return View();
        }

        // POST: Admin/VariantAttributeValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VarinatId,AttrbuteValueId")] VariantAttributeValue variantAttributeValue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variantAttributeValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttrbuteValueId"] = new SelectList(_context.ProductAttributeValues, "ValueId", "Value", variantAttributeValue.AttrbuteValueId);
            ViewData["VarinatId"] = new SelectList(_context.ProductVariants, "VariantId", "SKU", variantAttributeValue.VarinatId);
            return View(variantAttributeValue);
        }

        // GET: Admin/VariantAttributeValues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variantAttributeValue = await _context.VariantAttributeValues.FindAsync(id);
            if (variantAttributeValue == null)
            {
                return NotFound();
            }
            ViewData["AttrbuteValueId"] = new SelectList(_context.ProductAttributeValues, "ValueId", "Value", variantAttributeValue.AttrbuteValueId);
            ViewData["VarinatId"] = new SelectList(_context.ProductVariants, "VariantId", "SKU", variantAttributeValue.VarinatId);
            return View(variantAttributeValue);
        }

        // POST: Admin/VariantAttributeValues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VarinatId,AttrbuteValueId")] VariantAttributeValue variantAttributeValue)
        {
            if (id != variantAttributeValue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variantAttributeValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VariantAttributeValueExists(variantAttributeValue.Id))
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
            ViewData["AttrbuteValueId"] = new SelectList(_context.ProductAttributeValues, "ValueId", "Value", variantAttributeValue.AttrbuteValueId);
            ViewData["VarinatId"] = new SelectList(_context.ProductVariants, "VariantId", "SKU", variantAttributeValue.VarinatId);
            return View(variantAttributeValue);
        }

        // GET: Admin/VariantAttributeValues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variantAttributeValue = await _context.VariantAttributeValues
                .Include(v => v.ProductAttributeValue)
                .Include(v => v.ProductVariant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variantAttributeValue == null)
            {
                return NotFound();
            }

            return View(variantAttributeValue);
        }

        // POST: Admin/VariantAttributeValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variantAttributeValue = await _context.VariantAttributeValues.FindAsync(id);
            if (variantAttributeValue != null)
            {
                _context.VariantAttributeValues.Remove(variantAttributeValue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VariantAttributeValueExists(int id)
        {
            return _context.VariantAttributeValues.Any(e => e.Id == id);
        }
    }
}
