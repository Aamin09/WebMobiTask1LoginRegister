using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Controllers.User
{
    public class UserController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private const int PageSize = 12;

        public UserController(WebMobiTask1DbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Where(p => p.Status)
                .Take(8)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Products(int? categoryId, int? subcategoryId, decimal? minPrice, decimal? maxPrice, string sortOrder, int page = 1)
        {
            var data = context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Where(p => p.Status);

            // category filter
            if (categoryId.HasValue)
            {
                data = data.Where(p => p.CategoryId == categoryId);
            }

            // subcategory filter
            if (subcategoryId.HasValue)
            {
                data = data.Where(p => p.SubcategoryId == subcategoryId);
            }

            // price range filter
            if (minPrice.HasValue)
            {
                data = data.Where(p => p.CalculatedSellingPrice >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                data = data.Where(p => p.CalculatedSellingPrice <= maxPrice);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(p => p.Name);
                    break;
                case "price_asc":
                    data = data.OrderBy(p => p.CalculatedSellingPrice);
                    break;
                case "price_desc":
                    data = data.OrderByDescending(p => p.CalculatedSellingPrice);
                    break;
                default:
                    data = data.OrderBy(p => p.Name);
                    break;
            }

            // pagination logic code
            var totalProducts = await data.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);

            page = Math.Max(1, Math.Min(page, totalPages));

            var products = await data
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // categories data the dropdown
            var categories = await context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            // subcategories data for the selected category
            var subcategories = categoryId.HasValue
                ? await context.Subcategories
                    .Where(s => s.CategoryId == categoryId)
                    .Select(s => new SelectListItem
                    {
                        Value = s.SubcategoryId.ToString(),
                        Text = s.Name
                    })
                    .ToListAsync()
                : new List<SelectListItem>();


            var finalData = new ProductListViewModel
            {
                Products = products,
                Categories = new SelectList(categories, "Value", "Text", categoryId),
                Subategories = new SelectList(subcategories, "Value", "Text", subcategoryId),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CurrentPage = page,
                TotalPages = totalPages,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId
            };

            // ViewBag for sorting options
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CategoryId = categoryId;
            ViewBag.SubcategoryId = subcategoryId;
            ViewBag.SortOptions = new List<SelectListItem> {
                new SelectListItem { Text = "Name (A-Z)", Value = "name_asc" },   
                new SelectListItem { Text = "Name (Z-A)", Value = "name_desc" },
                new SelectListItem { Text = "Price (Low to High)", Value = "price_asc" },
                new SelectListItem { Text = "Price (High to Low)", Value = "price_desc" }
            };


            return View(finalData);
        }

        [HttpGet]
        public async Task<JsonResult> GetSubcategories(int categoryId)
        {
            var subcategories = await context.Subcategories
                .Where(s => s.CategoryId == categoryId)
                .Select(s => new { value = s.SubcategoryId, text = s.Name })
                .ToListAsync();

            return Json(subcategories);
        }

        [HttpGet]
        public async Task<JsonResult> GetPriceRange()
        {
            var priceRange = await context.Products
                .Where(p => p.Status)
                .Select(p => new
                {
                    MinPrice = context.Products.Min(p => p.CalculatedSellingPrice),
                    MaxPrice = context.Products.Max(p => p.CalculatedSellingPrice)
                })
                .FirstOrDefaultAsync();

            return Json(priceRange);
        }
    }
}