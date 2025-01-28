using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Controllers
{
    [Authorize]
    public class CategorySubcategoryController : Controller
    {
        private readonly WebMobiTask1DbContext context;

        public CategorySubcategoryController(WebMobiTask1DbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await context.Categories
                .Include(c => c.Subcategories).ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new CategorySubcategoryViewModel
            {
                Category = new Category(),
                Subcategories = new List<Subcategory> { new Subcategory() }
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategorySubcategoryViewModel model, string subcategoryNames)
        {
            if (ModelState.IsValid)
            {
                // Check if the category already exists
                var existingCategory = await context.Categories
                    .FirstOrDefaultAsync(c => c.Name == model.Category.Name);

                if (existingCategory != null)
                {
                    ModelState.AddModelError("Category.Name", "A category with this name already exists.");
                    return View(model);
                }

                // Add new category
                context.Categories.Add(model.Category);
                await context.SaveChangesAsync();

                // Parse subcategory names from the comma-separated string and add them
                if (!string.IsNullOrEmpty(subcategoryNames))
                {
                    var subcategoryList = subcategoryNames.Split(',')
                        .Select(name => new Subcategory
                        {
                            Name = name.Trim(),
                            CategoryId = model.Category.CategoryId // Link the subcategory to the newly created category
                        })
                        .ToList();

                    foreach (var subcategory in subcategoryList)
                    {
                        // Check for duplicates in the same category
                        var existingSubcategory = await context.Subcategories
                            .FirstOrDefaultAsync(s => s.Name == subcategory.Name && s.CategoryId == model.Category.CategoryId);

                        if (existingSubcategory != null)
                        {
                            ModelState.AddModelError("Subcategory.Name", $"A subcategory with the name '{subcategory.Name}' already exists in this category.");
                            return View(model);
                        }
                    }

                    // Add subcategories
                    context.Subcategories.AddRange(subcategoryList);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            var data = new CategorySubcategoryViewModel
            {
                Category = category,
                Subcategories = category.Subcategories.ToList()
            };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategorySubcategoryViewModel model, string subcategoryNames)
        {
            if (ModelState.IsValid)
            {
                // Log or inspect the CategoryId value to ensure it's passed correctly
                if (model.Category.CategoryId == 0)
                {
                    ModelState.AddModelError("Category.CategoryId", "Invalid Category ID.");
                    return View(model);
                }

                var existingCategory = await context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == model.Category.CategoryId);

                // If the category does not exist, log and return an error
                if (existingCategory == null)
                {
                    ModelState.AddModelError("Category.Name", "Category not found.");
                    return View(model);
                }

                // Check if the category name has changed and if the new name is unique
                if (existingCategory.Name != model.Category.Name)
                {
                    var categoryWithNewName = await context.Categories
                        .FirstOrDefaultAsync(c => c.Name == model.Category.Name);

                    if (categoryWithNewName != null)
                    {
                        ModelState.AddModelError("Category.Name", "A category with this name already exists.");
                        return View(model);
                    }
                }

                // Update category name
                existingCategory.Name = model.Category.Name;
                await context.SaveChangesAsync();

                // Parse subcategory names from the comma-separated string
                if (!string.IsNullOrEmpty(subcategoryNames))
                {
                    var subcategoryList = subcategoryNames.Split(',')
                        .Select(name => new Subcategory
                        {
                            Name = name.Trim(),
                            CategoryId = model.Category.CategoryId // Link the subcategory to the existing category
                        })
                        .ToList();

                    // Remove existing subcategories that are no longer part of the list
                    var existingSubcategories = await context.Subcategories
                        .Where(s => s.CategoryId == model.Category.CategoryId)
                        .ToListAsync();

                    foreach (var subcategory in existingSubcategories)
                    {
                        // If the subcategory isn't in the updated list, delete it
                        if (!subcategoryList.Any(s => s.Name == subcategory.Name))
                        {
                            context.Subcategories.Remove(subcategory);
                        }
                    }

                    // Add new subcategories
                    foreach (var subcategory in subcategoryList)
                    {
                        var existingSubcategory = await context.Subcategories
                            .FirstOrDefaultAsync(s => s.Name == subcategory.Name && s.CategoryId == model.Category.CategoryId);

                        if (existingSubcategory == null)
                        {
                            context.Subcategories.Add(subcategory);
                        }
                    }

                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var category = await context.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubcategory(int subcategoryId, int categoryId)
        {
            var subcategory = await context.Subcategories
                .FirstOrDefaultAsync(s => s.SubcategoryId == subcategoryId && s.CategoryId == categoryId);

            if (subcategory != null)
            {
                context.Subcategories.Remove(subcategory);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Delete", new { id = categoryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await context.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category != null)
            {
                // Optionally, delete all subcategories associated with the category
                context.Subcategories.RemoveRange(category.Subcategories);
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}

  