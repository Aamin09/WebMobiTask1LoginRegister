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
    public class ProductAttributesController : Controller
    {
        private readonly WebMobiTask1DbContext _context;

        public ProductAttributesController(WebMobiTask1DbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductAttributes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductAttributes.ToListAsync());
        }

        // GET: Admin/ProductAttributes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttributes
                .FirstOrDefaultAsync(m => m.AttributeId == id);
            if (productAttribute == null)
            {
                return NotFound();
            }

            return View(productAttribute);
        }

        // GET: Admin/ProductAttributes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductAttributes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttributeId,Name,IsAcrive,CreatedAt")] ProductAttribute productAttribute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productAttribute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productAttribute);
        }

        // GET: Admin/ProductAttributes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttributes.FindAsync(id);
            if (productAttribute == null)
            {
                return NotFound();
            }
            return View(productAttribute);
        }

        // POST: Admin/ProductAttributes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttributeId,Name,IsAcrive,CreatedAt")] ProductAttribute productAttribute)
        {
            if (id != productAttribute.AttributeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productAttribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductAttributeExists(productAttribute.AttributeId))
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
            return View(productAttribute);
        }

        // GET: Admin/ProductAttributes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttributes
                .FirstOrDefaultAsync(m => m.AttributeId == id);
            if (productAttribute == null)
            {
                return NotFound();
            }

            return View(productAttribute);
        }

        // POST: Admin/ProductAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productAttribute = await _context.ProductAttributes.FindAsync(id);
            if (productAttribute != null)
            {
                _context.ProductAttributes.Remove(productAttribute);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductAttributeExists(int id)
        {
            return _context.ProductAttributes.Any(e => e.AttributeId == id);
        }
    }
}
