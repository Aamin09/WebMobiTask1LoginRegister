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
    public class ProductAttributeValueMappingsController : Controller
    {
        private readonly WebMobiTask1DbContext _context;

        public ProductAttributeValueMappingsController(WebMobiTask1DbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductAttributeValueMappings
        public async Task<IActionResult> Index()
        {
            var webMobiTask1DbContext = _context.ProductAttributeValueMappings.Include(p => p.Product).Include(p => p.ProductAttributeValue);
            return View(await webMobiTask1DbContext.ToListAsync());
        }

        // GET: Admin/ProductAttributeValueMappings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeValueMapping = await _context.ProductAttributeValueMappings
                .Include(p => p.Product)
                .Include(p => p.ProductAttributeValue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productAttributeValueMapping == null)
            {
                return NotFound();
            }

            return View(productAttributeValueMapping);
        }

        // GET: Admin/ProductAttributeValueMappings/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Description");
            ViewData["AttributeValueId"] = new SelectList(_context.ProductAttributeValues, "ValueId", "Value");
            return View();
        }

        // POST: Admin/ProductAttributeValueMappings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,AttributeValueId")] ProductAttributeValueMapping productAttributeValueMapping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productAttributeValueMapping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Description", productAttributeValueMapping.ProductId);
            ViewData["AttributeValueId"] = new SelectList(_context.ProductAttributeValues, "ValueId", "Value", productAttributeValueMapping.AttributeValueId);
            return View(productAttributeValueMapping);
        }

        // GET: Admin/ProductAttributeValueMappings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeValueMapping = await _context.ProductAttributeValueMappings.FindAsync(id);
            if (productAttributeValueMapping == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Description", productAttributeValueMapping.ProductId);
            ViewData["AttributeValueId"] = new SelectList(_context.ProductAttributeValues, "ValueId", "Value", productAttributeValueMapping.AttributeValueId);
            return View(productAttributeValueMapping);
        }

        // POST: Admin/ProductAttributeValueMappings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,AttributeValueId")] ProductAttributeValueMapping productAttributeValueMapping)
        {
            if (id != productAttributeValueMapping.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productAttributeValueMapping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductAttributeValueMappingExists(productAttributeValueMapping.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Description", productAttributeValueMapping.ProductId);
            ViewData["AttributeValueId"] = new SelectList(_context.ProductAttributeValues, "ValueId", "Value", productAttributeValueMapping.AttributeValueId);
            return View(productAttributeValueMapping);
        }

        // GET: Admin/ProductAttributeValueMappings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeValueMapping = await _context.ProductAttributeValueMappings
                .Include(p => p.Product)
                .Include(p => p.ProductAttributeValue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productAttributeValueMapping == null)
            {
                return NotFound();
            }

            return View(productAttributeValueMapping);
        }

        // POST: Admin/ProductAttributeValueMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productAttributeValueMapping = await _context.ProductAttributeValueMappings.FindAsync(id);
            if (productAttributeValueMapping != null)
            {
                _context.ProductAttributeValueMappings.Remove(productAttributeValueMapping);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductAttributeValueMappingExists(int id)
        {
            return _context.ProductAttributeValueMappings.Any(e => e.Id == id);
        }
    }
}
