using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Interfaces;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class ProductsController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private readonly IImageService imageService;

        public ProductsController(WebMobiTask1DbContext context, IImageService imageService)
        {
            this.context = context;
            this.imageService = imageService;
        }
        // GET: Products
        public async Task<IActionResult> Index()
        {
            var data = await context.Products
                .Include(p => p.Category)
                .Include(p => p.OrderItems)
                .ThenInclude(oi=>oi.Order)
                .Include(p => p.Subcategory)
                .ThenInclude(g=>g.Taxes)
                .Include(p => p.ProductImages)
                .Include(p=>p.ProductVariants)
                .OrderByDescending(p=>p.ProductId).ToListAsync();
            return View(data);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Products
                   .Include(p => p.ProductImages)
                   .Include(p => p.Category)
                   .Include(p=>p.ProductVariants)
                   .Include(p => p.Subcategory)
                   .ThenInclude(g => g.Taxes)
                   .OrderByDescending(p => p.ProductId)
                   .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await context.Categories
              .Select(c => new SelectListItem
              {
                  Value = c.CategoryId.ToString(),
                  Text = c.Name
              })
              .ToListAsync();
            return View(new CreateProductsDto());
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductsDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string generatedSKU = $"C{model.CategoryId}S{model.SubcategoryId}{model.Name.Substring(0, 2).ToUpper()}{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}";
                  
                    var product = new Product
                    {
                        Name = model.Name,
                        Description = model.Description,
                        ShortDescription = model.ShortDescription,
                        CostPrice = model.CostPrice,
                        ProfitPercentage = model.ProfitPercentage,
                        SellingPricePercent = model.SellingPricePercentage,
                        SKU = generatedSKU,
                        Status = model.Status,
                        StockQuantity=model.StockQuantity,
                        MinimumStockLevel=model.MinimumStockLevel,
                        CategoryId = model.CategoryId,
                        SubcategoryId = model.SubcategoryId,
                        DeliveryCharge=model.DeliveryCharge,
                        HasVarinats=model.HasVariants,
                    };

                    product.CalculatePricing();

                    context.Add(product);
                    await context.SaveChangesAsync();

                    if (model.PrimaryImage != null)
                    {
                        var primaryImagePath = await imageService.SaveImage(model.PrimaryImage);
                        var primaryImage = new ProductImage
                        {
                            ProductId = product.ProductId,
                            ImageUrl = primaryImagePath,
                            IsPrimaryImage = true,
                            IsVariantImage=false,
                        };
                        context.ProductImages.Add(primaryImage);
                        await context.SaveChangesAsync();
                    }

                    if (model.GalleryImages != null && model.GalleryImages.Any())
                    {
                        foreach (var image in model.GalleryImages)
                        {
                            var galleryImagePath = await imageService.SaveImage(image);
                            var galleryImage = new ProductImage
                            {
                                ProductId = product.ProductId,
                                ImageUrl = galleryImagePath,
                                IsPrimaryImage = false,
                                IsVariantImage = false,
                            };
                            context.ProductImages.Add(galleryImage);
                        }
                        await context.SaveChangesAsync();
                    }

                    TempData["Success"] = "Product created successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the product.");
                // Log the exception details
                Console.WriteLine(ex);
            }

            await LoadDropdowns(model.CategoryId);

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetSubCategories(int categoryId)
        {
            var subcategories = await context.Subcategories
                .Where(s => s.CategoryId == categoryId)
                .Select(s => new { s.SubcategoryId, s.Name })
                .ToListAsync();
            return Json(subcategories);
        }


        // GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // loading data by productid
            var product = await context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            await LoadDropdowns(product.CategoryId);

            // model for exisiting product
            var model = new ProductDto
            {
                Id = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                CostPrice = product.CostPrice,
                ProfitPercentage=product.ProfitPercentage,
                SellingPricePercentage = product.SellingPricePercent,
                Status = product.Status,
                CategoryId = product.CategoryId,
                StockQuantity=product.StockQuantity,
                MinimumStockLevel=product.MinimumStockLevel,
                SubcategoryId = product.SubcategoryId,
                PrimaryImageUrl = product.ProductImages.FirstOrDefault(pi => pi.IsPrimaryImage && !pi.IsVariantImage)?.ImageUrl,
                DeliveryCharge=product.DeliveryCharge,
                HasVariants=product.HasVarinats,
            };

            // Reload existing images for the model
            await ReloadExistingImages(model);

            return View(model);
        }

        // POST: Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(model.CategoryId);
                await ReloadExistingImages(model);
                return View(model);
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var product = await context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.ProductId == model.Id);

                if (product == null)
                {
                    return NotFound();
                }

                if (product.Name != model.Name)
                {
                    string generatedSKU = $"C{model.CategoryId}S{model.SubcategoryId}{model.Name.Substring(0, 2).ToUpper()}{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}"; product.SKU = generatedSKU;
                }
                // updating the product table details 
                product.Name = model.Name;
                product.Description = model.Description;
                product.ShortDescription = model.ShortDescription;
                product.CostPrice = model.CostPrice;
                product.ProfitPercentage = model.ProfitPercentage;
                product.SellingPricePercent = model.SellingPricePercentage;
                product.Status = model.Status;
                product.CategoryId = model.CategoryId;
                product.SubcategoryId = model.SubcategoryId;
                product.DeliveryCharge= model.DeliveryCharge;
                product.StockQuantity = model.StockQuantity;
                product.MinimumStockLevel = model.MinimumStockLevel;
                product.HasVarinats = model.HasVariants;

                product.CalculatePricing();

                // deleting gallery image based on the checkbox selection 
                if (model.ImagesToDelete != null && model.ImagesToDelete.Any())
                {
                    var imagesToDelete = product.ProductImages
                        .Where(pi => model.ImagesToDelete.Contains(pi.Id) && !pi.IsPrimaryImage && !pi.IsVariantImage)
                        .ToList();

                    foreach (var image in imagesToDelete)
                    {
                        imageService.DeleteImage(image.ImageUrl);
                        context.ProductImages.Remove(image);
                    }
                    // saving changes 
                    await context.SaveChangesAsync();
                }

                // primary image update code if new upload 
                if (model.PrimaryImage != null)
                {
                    var oldPrimaryImage = product.ProductImages
                        .FirstOrDefault(pi => pi.IsPrimaryImage && !pi.IsVariantImage);

                    if (oldPrimaryImage != null)
                    {
                        var oldImageUrl = oldPrimaryImage.ImageUrl;
                        context.ProductImages.Remove(oldPrimaryImage);
                        await context.SaveChangesAsync(); // update into table
                        imageService.DeleteImage(oldImageUrl); // remove from folder
                    }

                    // Adding new primary image
                    var newPrimaryImagePath = await imageService.SaveImage(model.PrimaryImage);
                    var newPrimaryImage = new ProductImage
                    {
                        ProductId = product.ProductId,
                        ImageUrl = newPrimaryImagePath,
                        IsPrimaryImage = true,
                        IsVariantImage = false,
                    };
                    context.ProductImages.Add(newPrimaryImage);
                    await context.SaveChangesAsync();
                }

                //  Adding new gallery images
                if (model.GalleryImages != null && model.GalleryImages.Any())
                {
                    foreach (var imageFile in model.GalleryImages)
                    {
                        if (imageFile.Length > 0)
                        {
                            var galleryImagePath = await imageService.SaveImage(imageFile);
                            var galleryImage = new ProductImage
                            {
                                ProductId = product.ProductId,
                                ImageUrl = galleryImagePath,
                                IsPrimaryImage = false,
                                IsVariantImage = false,
                            };
                            context.ProductImages.Add(galleryImage);
                        }
                    }
                    await context.SaveChangesAsync();
                }

                // Final save for any remaining changes
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "An error occurred while updating the product.");
                Console.WriteLine($"Error updating product: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                await LoadDropdowns(model.CategoryId);
                await ReloadExistingImages(model);
                return View(model);
            }
        }

        // Add this new helper method to reload existing images
        public async Task ReloadExistingImages(ProductDto model)
        {
            var product = await context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == model.Id);

            if (product != null)
            {
                model.ExistingGalleryImages = product.ProductImages
                    .Where(pi => !pi.IsPrimaryImage && !pi.IsVariantImage) 
                    .Select(pi => new ProductImageDto
                    {
                        ProductImageId = pi.Id,
                        ImageUrl = pi.ImageUrl,
                    })
                    .ToList();
                var primaryImage = product.ProductImages.FirstOrDefault(pi => pi.IsPrimaryImage && !pi.IsVariantImage);
                if (primaryImage != null)
                {
                    model.PrimaryImageUrl = primaryImage.ImageUrl; // Assigning the primary image URL
                }

            }

        }


        public async Task LoadDropdowns(int? selectedCategoryId)
        {
            var categories = await context.Categories.ToListAsync();
            var subcategories = await context.Subcategories.ToListAsync();

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name", selectedCategoryId);
            ViewBag.Subcategories = new SelectList(subcategories, "SubcategoryId", "Name");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .ThenInclude(g=>g.Taxes)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductImage(int imageId, int productId)
        {
            var productImage = await context.ProductImages
                .FirstOrDefaultAsync(pi => pi.Id == imageId && pi.ProductId == productId);

            if (productImage != null)
            {
                imageService.DeleteImage(productImage.ImageUrl);
                context.ProductImages.Remove(productImage);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Delete", new { id = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await context.Products
                .Include(p => p.ProductImages)
                .Include(pv=>pv.ProductVariants)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product != null)
            {
                // Delete all product images associated with the product
                foreach (var image in product.ProductImages)
                {
                    imageService.DeleteImage(image.ImageUrl);
                    context.ProductImages.Remove(image);
                }
                context.ProductVariants.RemoveRange(product.ProductVariants);
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Status = !product.Status;

            await context.SaveChangesAsync();

            return Json(new { isActive = product.Status });
        }

    }
}
