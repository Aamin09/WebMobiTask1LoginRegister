using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
                .OrderByDescending(p => p.ProductId)
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
                "oldest" => data.OrderBy(p => p.ProductId),
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

        [HttpGet]
        public async Task<IActionResult> Details(int id, int? variantId)
        {
            var product = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Include(p => p.ProductImages)
                .Include(p => p.Reviews).ThenInclude(r => r.User)
                .Include(p=>p.ProductAttributeValueMappings).ThenInclude(pavm => pavm.ProductAttributeValue).ThenInclude(pav => pav.Attribute)
                .Include(p => p.ProductVariants).ThenInclude(pv => pv.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();

            // getting the variant product attributes
            var variantAttributeValues = await context.VariantAttributeValues
                .Include(vav => vav.ProductAttributeValue).ThenInclude(pav => pav.Attribute)
                .Where(vav => product.ProductVariants.Select(pv => pv.VariantId).Contains(vav.VariantId))
                .ToListAsync();

            // getting main product attributes
            var mainProductAttributeValues = product.ProductAttributeValueMappings
                .Select(pavm => new ProductAttributeDisplayDto
                {
                    AttributeName = pavm.ProductAttributeValue.Attribute.Name,
                    AttributeValue = pavm.ProductAttributeValue.Value,
                    VariantId=null,
                    IsProductAttribute=true
                }).ToList();

            ViewBag.VariantAttributeValues = variantAttributeValues;
            ViewBag.MainProductAttributeValues = mainProductAttributeValues;

            var selectedAttributes = Request.Query.Keys
                .Where(key => key.StartsWith("attr_"))
                .ToDictionary(key => key.Substring(5), key => (string)Request.Query[key]);

            ProductVariant selectedVariant = null;

            if (variantId.HasValue && variantId > 0)
            {
                selectedVariant = product.ProductVariants.FirstOrDefault(pv => pv.VariantId == variantId && pv.ProductId == id);
                if (selectedVariant != null && selectedAttributes.Count == 0)
                {
                    selectedAttributes = variantAttributeValues
                        .Where(v => v.VariantId == selectedVariant.VariantId)
                        .ToDictionary(v => v.ProductAttributeValue.Attribute.Name, v => v.ProductAttributeValue.Value);
                }
            }
            else if (selectedAttributes.Count == 0) {
                selectedAttributes = new Dictionary<string, string>();
                selectedVariant = null;
            }
            else
            {

                bool matchesMainProduct = selectedAttributes.All(attr =>
                        mainProductAttributeValues.Any(pav =>
                            pav.AttributeName == attr.Key &&
                            pav.AttributeValue == attr.Value));

                if (matchesMainProduct)
                {
                    selectedVariant = null; // Reset, it's not a variant selection
                }
                else
                {
                    selectedVariant = product.ProductVariants
                        .Where(variant => selectedAttributes.All(attr =>
                            variantAttributeValues.Any(vav =>
                                vav.VariantId == variant.VariantId &&
                                vav.ProductAttributeValue.Attribute.Name == attr.Key &&
                                vav.ProductAttributeValue.Value == attr.Value)))
                        .OrderByDescending(v => v.StockQuantity > 0)
                        .FirstOrDefault();

                }
            }

            ViewBag.SelectedVariant = selectedVariant;
            ViewBag.SelectedAttributes = selectedAttributes;

            // combine variant and product attributes for display
            var allAttributes = variantAttributeValues
             .Select(vav => new ProductAttributeDisplayDto
             {
                 AttributeName = vav.ProductAttributeValue.Attribute.Name,
                 AttributeValue = vav.ProductAttributeValue.Value,
                 VariantId = vav.VariantId,
                 IsProductAttribute = false
             })
             .Concat(mainProductAttributeValues)
             .ToList();

            var availableVariantsByAttribute = allAttributes
                .GroupBy(vav => vav.AttributeName)
                .ToDictionary(
                    group => group.Key,
                    group => group.GroupBy(vav => vav.AttributeValue)
                    .ToDictionary(
                        valueGroup => valueGroup.Key,
                        valueGroup => valueGroup.Select(v => v.VariantId).Distinct().ToList()
                     )
                 );

            var attributeGroups = availableVariantsByAttribute.ToDictionary(
                kvp => kvp.Key,
                kvp =>
                {
                    var currentAttrName = kvp.Key;
                    var attrValues = kvp.Value;

                    return attrValues.Select(valueEntry =>
                    {
                        var variantIds = valueEntry.Value;
                        var isProductAttribute = variantIds.All(vid => vid == null);

                        var anyAvailable = isProductAttribute  || variantIds.Any(vid => product.ProductVariants.Any(pv => pv.VariantId == vid && pv.StockQuantity > 0));
                        var isCompatible = selectedAttributes.Count == 0 || selectedAttributes.All(selctedAttr =>
                            selctedAttr.Key == currentAttrName
                            || (isProductAttribute && selctedAttr.Key == currentAttrName) || variantIds.Any(vid =>
                            allAttributes.Any(vav => vav.VariantId == vid &&
                                                        vav.AttributeName == selctedAttr.Key &&
                                                        vav.AttributeValue == selctedAttr.Value)));

                        return new
                        {
                            ValueName = valueEntry.Key,
                            VariantIds = variantIds,
                            IsAvailable = anyAvailable,
                            IsCompatible = isCompatible,
                            IsProductAttribute = isProductAttribute,
                        };
                    }).ToList<dynamic>();
                });

            ViewBag.AttributeGroups = attributeGroups;
            ViewBag.relatedProducts = await context.Products
              .Include(p => p.ProductImages)
              .Where(p => p.CategoryId == product.CategoryId && p.ProductId != id)
              .Take(4)
              .ToListAsync();

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