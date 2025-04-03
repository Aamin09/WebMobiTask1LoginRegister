using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Task1LoginRegister.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private const int PageSize = 12;

        public HomeController(WebMobiTask1DbContext context)
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
                .OrderByDescending(p=>p.ProductId)
                .Take(8)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Products(int? categoryId, string subcategoryIds, decimal? minPrice,
     decimal? maxPrice, string sortOrder, string searchProduct, int page = 1)
        {
            var data = context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Where(p => p.Status);


            if (categoryId.HasValue)
            {
                data = data.Where(p => p.CategoryId == categoryId).OrderByDescending(p => p.ProductId);
            }

            if (!string.IsNullOrEmpty(subcategoryIds))
            {
                var selectedIds = subcategoryIds.Split(',')
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(int.Parse)
                    .ToList();

                if (selectedIds.Any())
                {
                    data = data.Where(p => selectedIds.Contains(p.SubcategoryId)).OrderByDescending(p => p.ProductId);
                }
            }

            if (!string.IsNullOrEmpty(searchProduct))
            {
                data = data.Where(p => p.Name.ToLower().StartsWith(searchProduct.ToLower()) || p.Description.ToLower().StartsWith(searchProduct.ToLower())).OrderByDescending(p => p.ProductId);
            }

            if (minPrice.HasValue)
            {
                data = data.Where(p => p.CalculatedSellingPrice >= minPrice).OrderByDescending(p => p.ProductId);
            }
            if (maxPrice.HasValue)
            {
                data = data.Where(p => p.CalculatedSellingPrice <= maxPrice).OrderByDescending(p => p.ProductId);
            }

            // sorting
            data = sortOrder switch
            {
                "oldest"=> data.OrderBy(p => p.ProductId),
                "name_asc" => data.OrderBy(p => p.Name),
                "name_desc" => data.OrderByDescending(p => p.Name),
                "price_asc" => data.OrderBy(p => p.CalculatedSellingPrice),
                "price_desc" => data.OrderByDescending(p => p.CalculatedSellingPrice),
                _ => data.OrderByDescending(p => p.ProductId)
            };

            var totalItems = await data.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            // paginated results
            var products = await data
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // categories for dropdown
            var categories = await context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            //  subcategories for selected category
            var subcategories = await context.Subcategories
                .Where(s => categoryId == null || s.CategoryId == categoryId)
                .Select(s => new SelectListItem
                {
                    Value = s.SubcategoryId.ToString(),
                    Text = s.Name
                })
                .ToListAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                Categories = new SelectList(categories, "Value", "Text", categoryId),
                Subcategories = new SelectList(subcategories, "Value", "Text"),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CurrentPage = page,
                TotalPages = totalPages,
                CategoryId = categoryId,
                SubcategoryIds = subcategoryIds,
                SearchProductname = searchProduct
            };

            // Set up ViewBag data
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Subcategories = subcategories;
            ViewBag.SortOptions = new List<SelectListItem> {
                new SelectListItem { Text = "Newest", Value = "newsest" },
        new SelectListItem { Text = "Oldest", Value = "oldest" },

        new SelectListItem { Text = "Name (A-Z)", Value = "name_asc" },
        new SelectListItem { Text = "Name (Z-A)", Value = "name_desc" },
        new SelectListItem { Text = "Price (Low to High)", Value = "price_asc" },
        new SelectListItem { Text = "Price (High to Low)", Value = "price_desc" }
    };

            return View(viewModel);
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

        // details page code 
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                 .Include(p => p.Reviews)
                 .ThenInclude(r=>r.User)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            // getting relted products
            var relatedProducts = await context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != id)
                .Take(4)
                .ToListAsync();


            ViewBag.relatedProducts = relatedProducts;

            return View(product);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}