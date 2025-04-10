using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductAttributeValuesController : Controller
    {
        private readonly WebMobiTask1DbContext _context;

        public ProductAttributeValuesController(WebMobiTask1DbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductAttributeValues
        public async Task<IActionResult> Index()
        {
            var webMobiTask1DbContext = _context.ProductAttributeValues.Include(p => p.Attribute);
            return View(await webMobiTask1DbContext.ToListAsync());
        }

        // GET: Admin/ProductAttributeValues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeValue = await _context.ProductAttributeValues
                .Include(p => p.Attribute)
                .FirstOrDefaultAsync(m => m.ValueId == id);
            if (productAttributeValue == null)
            {
                return NotFound();
            }

            return View(productAttributeValue);
        }

        // GET: Admin/ProductAttributeValues/Create
        public IActionResult Create()
        {
            ViewData["AttributeId"] = new SelectList(_context.ProductAttributes, "AttributeId", "Name");
            return View();
        }

        // POST: Admin/ProductAttributeValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ValueId,AttributeId,Value,IsActive,CreatedAt")] ProductAttributeValue productAttributeValue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productAttributeValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttributeId"] = new SelectList(_context.ProductAttributes, "AttributeId", "Name", productAttributeValue.AttributeId);
            return View(productAttributeValue);
        }

        // GET: Admin/ProductAttributeValues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeValue = await _context.ProductAttributeValues.FindAsync(id);
            if (productAttributeValue == null)
            {
                return NotFound();
            }
            ViewData["AttributeId"] = new SelectList(_context.ProductAttributes, "AttributeId", "Name", productAttributeValue.AttributeId);
            return View(productAttributeValue);
        }

        // POST: Admin/ProductAttributeValues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ValueId,AttributeId,Value,IsActive,CreatedAt")] ProductAttributeValue productAttributeValue)
        {
            if (id != productAttributeValue.ValueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productAttributeValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductAttributeValueExists(productAttributeValue.ValueId))
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
            ViewData["AttributeId"] = new SelectList(_context.ProductAttributes, "AttributeId", "Name", productAttributeValue.AttributeId);
            return View(productAttributeValue);
        }

        // GET: Admin/ProductAttributeValues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeValue = await _context.ProductAttributeValues
                .Include(p => p.Attribute)
                .FirstOrDefaultAsync(m => m.ValueId == id);
            if (productAttributeValue == null)
            {
                return NotFound();
            }

            return View(productAttributeValue);
        }

        // POST: Admin/ProductAttributeValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productAttributeValue = await _context.ProductAttributeValues.FindAsync(id);
            if (productAttributeValue != null)
            {
                _context.ProductAttributeValues.Remove(productAttributeValue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductAttributeValueExists(int id)
        {
            return _context.ProductAttributeValues.Any(e => e.ValueId == id);
        }
    }
}
