using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategorySubcategoryController : Controller
    {
        private readonly WebMobiTask1DbContext context;

        public CategorySubcategoryController(WebMobiTask1DbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await context.Categories.OrderByDescending(c => c.CategoryId)
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
        public async Task<IActionResult> Create(CategorySubcategoryViewModel model, List<string> SubcategoryNames)
        {
            if (ModelState.IsValid)
            {
                // Check if the category already exists
                var existingCategory = await context.Categories
                    .FirstOrDefaultAsync(c => c.Name == model.Category.Name);

                if (existingCategory != null)
                {
                    ViewBag.errorExisting = $"<script>alert('{existingCategory.Name} : A category with this name already exists.)</script>'";
                    return View(model);
                }

                // Add new category
                context.Categories.Add(model.Category);
                await context.SaveChangesAsync();

                // Add multiple subcategories dynamically
                if (SubcategoryNames != null && SubcategoryNames.Any())
                {
                    var subcategoryList = SubcategoryNames
                        .Where(name => !string.IsNullOrWhiteSpace(name))
                        .Select(name => new Subcategory
                        {
                            Name = name.Trim(),
                            CategoryId = model.Category.CategoryId
                        })
                        .ToList();

                    context.Subcategories.AddRange(subcategoryList);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategorySubcategoryViewModel
            {
                Category = category,
                Subcategories = category.Subcategories.ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(CategorySubcategoryViewModel model, string[] updatedSubcategoryNames, int[] ExistingSubcategoryIds, int[] SubcategoryIdsToRemove, string[] NewSubcategoryNames)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = await context.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.CategoryId == model.Category.CategoryId);

            if (category == null)
            {
                return NotFound();
            }

            // updating category name
            category.Name = model.Category.Name;

            // removing selected Subcategories
            if (SubcategoryIdsToRemove != null)
            {
                foreach (var subcategoryId in SubcategoryIdsToRemove)
                {
                    var subcategoryToRemove = category.Subcategories.FirstOrDefault(s => s.SubcategoryId == subcategoryId);
                    if (subcategoryToRemove != null)
                    {
                        category.Subcategories.Remove(subcategoryToRemove);
                    }
                }
            }

            // Converting ICollection to List for Indexing
            var subcategoryList = category.Subcategories.ToList();

            // Updating Existing Subcategory Names
            if (updatedSubcategoryNames != null && ExistingSubcategoryIds != null)
            {
                for (int i = 0; i < ExistingSubcategoryIds.Length; i++)
                {
                    var subcategory = subcategoryList.FirstOrDefault(s => s.SubcategoryId == ExistingSubcategoryIds[i]);

                    if (subcategory != null && !string.IsNullOrEmpty(updatedSubcategoryNames[i]) && subcategory.Name != updatedSubcategoryNames[i])
                    {
                        subcategory.Name = updatedSubcategoryNames[i];
                    }
                }
            }

            // Adding  New Subcategories code
            if (NewSubcategoryNames != null)
            {
                foreach (var newName in NewSubcategoryNames)
                {
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        category.Subcategories.Add(new Subcategory { Name = newName });
                    }
                }
            }


            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

