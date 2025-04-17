using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Interfaces;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductVariantController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private readonly IImageService imageService;

        public ProductVariantController(WebMobiTask1DbContext context, IImageService imageService)
        {
            this.context = context;
            this.imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var variants = await context.ProductVariants
                .Include(pv => pv.Product).ToListAsync();
            return View(variants);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var variants = await context.ProductVariants
                .Include(pv => pv.Product).FirstOrDefaultAsync(pv => pv.VariantId == id);

            if (variants == null) return NotFound();

            return View(variants);
        }

        public async Task<IActionResult> Create(int productId)
        {
            var product = await context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var productAttributes = await context.ProductAttributes
                .Where(a => a.IsActive).Include(a => a.ProductAttributeValue)
                .ToListAsync();

            // creating a single variant initialiazed with product data
            var initialVariant = new ProductVariantDto();
            initialVariant.InitializeFromBaseProductPrice(product);

            var finalData = new ProductVariantCreateDto
            {
                ProductId = productId,
                ProductName = product.Name,
                Variants = new List<ProductVariantDto> { initialVariant },
                AvailableAttributes = productAttributes
            };

            return View(finalData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVariantCreateDto model)
        {
            var productAttributes = await context.ProductAttributes
            .Where(a => a.IsActive)
            .Include(a => a.ProductAttributeValue).ToListAsync();

            model.AvailableAttributes = productAttributes;
            bool attributesExists = productAttributes.Any();

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await context.Products.FindAsync(model.ProductId);
                    if (product == null) return NotFound();

                    bool hasErrors = false;
                    List<string> errorMessages = new List<string>();

                    foreach (var variantDto in model.Variants)
                    {
                        variantDto.CalculatePricing();

                        if (!variantDto.IsValidPricing())
                        {
                            TempData["error"] = $"Invalid pricing for variant '{variantDto.VarinatName}'. Final selling price must be greater than cost price.";
                            var productAttribute = await context.ProductAttributes
                              .Where(a => a.IsActive)
                              .Include(a => a.ProductAttributeValue).ToListAsync();
                            model.AvailableAttributes = productAttribute;
                            return View(model);
                        }
                        // Check if AttributeValueIds is properly initialized
                        if (variantDto.AttributeValueIds == null)
                        {
                            variantDto.AttributeValueIds = new List<int>();
                        }

                        // validate that at least one attribute is selected if attributes exist
                        if (attributesExists && (variantDto.AttributeValueIds == null || !variantDto.AttributeValueIds.Any()))
                        {
                            errorMessages.Add($"At least one attribute value must be selected for variant '{variantDto.VarinatName}'.");
                            hasErrors = true;
                        }
                        // creating new variant from dto
                        var variant = new ProductVariant
                        {
                            ProductId = model.ProductId,
                            VarinatName = variantDto.VarinatName,
                            VarinatDescription = variantDto.VarinatDescription,
                            SKU = $"P{model.ProductId}V{variantDto.VarinatName.Substring(0, 3)}{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}",
                            CostPrice = variantDto.CostPrice,
                            ProfitPercentage = variantDto.ProfitPercentage,
                            BasePrice = variantDto.BasePrice,
                            DiscountPercentage = variantDto.DiscountPercentage,
                            FinalSellingPrice = variantDto.FinalSellingPrice,
                            StockQuantity = variantDto.StockQuantity,
                            IsActive = variantDto.IsActive,
                            CreatedAt = DateTime.Now
                        };

                        // adding varinat 
                        context.Add(variant);
                        await context.SaveChangesAsync();

                        // save attribute values for this variant
                        if (variantDto.AttributeValueIds != null && variantDto.AttributeValueIds.Any())
                        {
                            foreach (var attributeValueId in variantDto.AttributeValueIds)
                            {
                                var variantAttributeValue = new VariantAttributeValue
                                {
                                    VariantId = variant.VariantId,
                                    AttributeValueId = attributeValueId
                                };
                                context.Add(variantAttributeValue);
                            }
                            await context.SaveChangesAsync();
                        }

                        // variant primary image
                        if (variantDto.VarinatPrimaryImage != null)
                        {
                            var primaryImagePath = await imageService.SaveImage(variantDto.VarinatPrimaryImage, "variants");
                            var primaryImage = new ProductImage
                            {
                                ProductId = model.ProductId,
                                ImageUrl = primaryImagePath,
                                IsPrimaryImage = true,
                                VariantId = variant.VariantId,
                                IsVariantImage = true,
                                CreatedAt = DateTime.Now
                            };
                            context.ProductImages.Add(primaryImage);
                            await context.SaveChangesAsync();
                        }
                        // varinat galleryimage
                        if (variantDto.VarinatGalleryImages != null && variantDto.VarinatGalleryImages.Any())
                        {
                            foreach (var image in variantDto.VarinatGalleryImages)
                            {
                                var galleryImagePath = await imageService.SaveImage(image, "variants");
                                var galleryImage = new ProductImage
                                {
                                    ProductId = model.ProductId,
                                    ImageUrl = galleryImagePath,
                                    IsPrimaryImage = false,
                                    VariantId = variant.VariantId,
                                    IsVariantImage = true,
                                    CreatedAt = DateTime.Now
                                };
                                context.ProductImages.Add(galleryImage);
                            }
                            await context.SaveChangesAsync();
                        }

                        if (hasErrors)
                        {
                            TempData["error"] = string.Join("<br>", errorMessages);
                            return View(model);
                        }
                        product.HasVarinats = true;
                        context.Update(product);
                        await context.SaveChangesAsync();

                        return RedirectToAction("Details", "Products", new { id = model.ProductId });
                    }
                    TempData["success"] = "Product variants created successfully!";
                    return RedirectToAction("Details", "ProductVariant", new { id = model.ProductId });
                }
                catch (Exception ex)
                {
                    TempData["error"] = "An error occurred while saving variants: " + ex.Message;
                }
            }
            else
            {
                // Add ModelState errors to TempData
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["error"] = string.Join(", ", errors);
            }


            return View(model);
        }

        public async Task<IActionResult> AddVariantForm(int index, int productId)
        {
            var product = await context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var variant = new ProductVariantDto();
            variant.InitializeFromBaseProductPrice(product);

            var productAttributes = await context.ProductAttributes
                .Where(a => a.IsActive).Include(a => a.ProductAttributeValue).ToListAsync();

            ViewBag.VariantIndex = index;
            ViewBag.ProductId = productId;
            ViewBag.ProductAttributes = productAttributes;

            return PartialView("_VariantForm", variant);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var variants = await context.ProductVariants
                .Include(pv => pv.Product).FirstOrDefaultAsync(pv => pv.VariantId == id);

            if (variants == null) return NotFound();

            var variantAttributeValues = await context.VariantAttributeValues
                .Where(vav => vav.VariantId == id).ToListAsync();

            var productAttributes = await context.ProductAttributes
                .Where(a => a.IsActive).Include(a => a.ProductAttributeValue).ToListAsync();

            var variantImages = await context.ProductImages
                .Where(img => img.VariantId == id).ToListAsync();

            // dto for exisitng fields
            var variantDto = new ProductVariantDto
            {
                VarinatName = variants.VarinatName,
                VarinatDescription = variants.VarinatDescription,
                CostPrice = variants.CostPrice,
                ProfitPercentage = variants.ProfitPercentage,
                BasePrice = variants.BasePrice,
                DiscountPercentage = variants.DiscountPercentage,
                FinalSellingPrice = variants.FinalSellingPrice,
                StockQuantity = variants.StockQuantity,
                IsActive = variants.IsActive,
                AttributeValueIds = variantAttributeValues.Select(vav => vav.AttributeValueId).ToList()
            };
            // model for exisiting product
            var model = new ProductVariantEditDto
            {
                VariantId = variants.VariantId,
                ProductId = variants.ProductId,
                ProductName = variants.Product.Name,
                Variant = variantDto,
                AvailableAttributes = productAttributes,
                ExistingImages = variantImages,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVariantEditDto model)
        {
            if (id != model.VariantId) return NotFound();

            var productAttributes = await context.ProductAttributes
                .Where(a => a.IsActive).Include(a => a.ProductAttributeValue).ToListAsync();

            model.AvailableAttributes = productAttributes;
            bool attributeExists = productAttributes.Any();

            if (ModelState.IsValid)
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    var variant = await context.ProductVariants.FindAsync(id);
                    if (variant == null) return NotFound();
                    var variantImages = await context.ProductImages
                       .Where(img => img.VariantId == id).ToListAsync();
                    model.ExistingImages = variantImages;
                    model.Variant.CalculatePricing();
                    if (!model.Variant.IsValidPricing())
                    {
                        TempData["error"] = $"Invalid pricing for variant '{model.Variant.VarinatName}'. Final selling price must be greater than cost price.";
                        model.ExistingImages = variantImages;

                        return View(model);
                    }

                    variant.VarinatName = model.Variant.VarinatName;
                    variant.VarinatDescription = model.Variant.VarinatDescription;
                    variant.CostPrice = model.Variant.CostPrice;
                    variant.ProfitPercentage = model.Variant.ProfitPercentage;
                    variant.BasePrice = model.Variant.BasePrice;
                    variant.DiscountPercentage = model.Variant.DiscountPercentage;
                    variant.FinalSellingPrice = model.Variant.FinalSellingPrice;
                    variant.StockQuantity = model.Variant.StockQuantity;
                    variant.IsActive = model.Variant.IsActive;
                    variant.UpdatedAt = DateTime.UtcNow;

                    context.Update(variant);
                    await context.SaveChangesAsync();

                    // hadle attribute values
                    if (attributeExists && (model.Variant.AttributeValueIds == null || !model.Variant.AttributeValueIds.Any()))
                    {
                        TempData["error"] = $"At least one attribute value must be selected for variant '{model.Variant.VarinatName}'.";

                        // Reload existing images
                        model.ExistingImages = variantImages;

                        return View(model);
                    }

                    // remove existing attribute values
                    var exisitingAttributeValues = await context.VariantAttributeValues
                        .Where(vav => vav.VariantId == id).ToListAsync();
                    context.VariantAttributeValues.RemoveRange(exisitingAttributeValues);
                    await context.SaveChangesAsync();

                    // add new attribute values
                    if (model.Variant.AttributeValueIds != null && model.Variant.AttributeValueIds.Any())
                    {
                        foreach (var attributeValueId in model.Variant.AttributeValueIds)
                        {
                            var variantAttributeValues = new VariantAttributeValue
                            {
                                VariantId = variant.VariantId,
                                AttributeValueId = attributeValueId,
                            };
                            context.Add(variantAttributeValues);
                        }
                        await context.SaveChangesAsync();
                    }

                    // handle image deleion
                    if (model.Variant.ImagesToDelete != null && model.Variant.ImagesToDelete.Any())
                    {
                        var imagesToDelete = await context.ProductImages
                            .Where(pi => model.Variant.ImagesToDelete.Contains(pi.Id) && pi.VariantId == id).ToListAsync();

                        foreach (var image in imagesToDelete)
                        {
                            imageService.DeleteImage(image.ImageUrl);
                            context.ProductImages.Remove(image);
                        }
                        await context.SaveChangesAsync();
                    }

                    // primary image update code if new upload 
                    if (model.Variant.VarinatPrimaryImage != null)
                    {
                        var oldPrimaryImage = await context.ProductImages
                            .FirstOrDefaultAsync(pi => pi.VariantId == id && pi.IsVariantImage && pi.IsPrimaryImage);

                        if (oldPrimaryImage != null)
                        {
                            var oldImageUrl = oldPrimaryImage.ImageUrl;
                            context.ProductImages.Remove(oldPrimaryImage);
                            await context.SaveChangesAsync();
                            imageService.DeleteImage(oldImageUrl);
                        }

                        // Adding new primary image
                        var newPrimaryImagePath = await imageService.SaveImage(model.Variant.VarinatPrimaryImage, "variants");
                        var newPrimaryImage = new ProductImage
                        {
                            ProductId = model.ProductId,
                            ImageUrl = newPrimaryImagePath,
                            IsPrimaryImage = true,
                            IsVariantImage = true,
                            VariantId = variant.VariantId,
                            CreatedAt = DateTime.Now,
                        };
                        context.ProductImages.Add(newPrimaryImage);
                        await context.SaveChangesAsync();
                    }

                    //  Adding new gallery images
                    if (model.Variant.VarinatGalleryImages != null && model.Variant.VarinatGalleryImages.Any())
                    {
                        foreach (var imageFile in model.Variant.VarinatGalleryImages)
                        {
                            if (imageFile.Length > 0)
                            {
                                var galleryImagePath = await imageService.SaveImage(imageFile, "variants");
                                var galleryImage = new ProductImage
                                {
                                    ProductId = model.ProductId,
                                    ImageUrl = galleryImagePath,
                                    IsPrimaryImage = false,
                                    IsVariantImage = true,
                                    VariantId = variant.VariantId,
                                    CreatedAt = DateTime.Now
                                };
                                context.ProductImages.Add(galleryImage);
                            }
                        }
                        await context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    TempData["success"] = "Product variant updated successfully!";
                    return RedirectToAction("Index", "ProductVariant", new { id = variant.VariantId });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["error"] = "An error occurred while updating the variant: " + ex.Message;

                    // Reload existing images
                    var variantImages = await context.ProductImages
                        .Where(img => img.VariantId == id).ToListAsync();
                    model.ExistingImages = variantImages;

                    return View(model);
                }
            }
            else
            {
                // Add ModelState errors to TempData
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["error"] = string.Join(", ", errors);

                // Reload existing images
                var variantImages = await context.ProductImages
                    .Where(img => img.VariantId == id).ToListAsync();
                model.ExistingImages = variantImages;
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var variants = await context.ProductVariants
                .Include(pv => pv.Product).Include(pv=>pv.ProductImages).FirstOrDefaultAsync(pv => pv.VariantId == id);

            if (variants == null) return NotFound();

            return View(variants);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVariantImage(int imageId, int variantId)
        {
            var productImage = await context.ProductImages
                .FirstOrDefaultAsync(pi => pi.Id == imageId && pi.VariantId == variantId);

            if (productImage != null)
            {
                imageService.DeleteImage(productImage.ImageUrl);
                context.ProductImages.Remove(productImage);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Delete", new { id = variantId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVariant(int id)
        {
            var variants = await context.ProductVariants
                .Include(pv => pv.ProductImages)
                .FirstOrDefaultAsync(pv => pv.VariantId == id);

            if (variants == null) return NotFound();

            bool hasOrders = await context.OrderItems.AnyAsync(oi => oi.ProductVariantId == id);

            if (hasOrders)
            {
                // Soft delete
                variants.IsActive = false; // Mark it as inactive
                context.Update(variants);
                TempData["WarningMessage"] = "This variant is linked to existing orders and cannot be deleted. It has been marked as inactive to preserve order history.";
            }
            else
            {
                var variantAttributeValues = await context.VariantAttributeValues
                    .Where(vav => vav.VariantId == id).ToListAsync();

                context.VariantAttributeValues.RemoveRange(variantAttributeValues);

                foreach (var image in variants.ProductImages)
                {
                    imageService.DeleteImage(image.ImageUrl);
                    context.ProductImages.Remove(image);
                }

                context.ProductVariants.Remove(variants);
            }

            await context.SaveChangesAsync();

            // Check if product still has any active variants
            var hasVariants = await context.ProductVariants
                .AnyAsync(v => v.ProductId == variants.ProductId && v.IsActive == true);

            if (!hasVariants)
            {
                var product = await context.Products.FindAsync(variants.ProductId);
                if (product != null)
                {
                    product.HasVarinats = false;
                    context.Update(product);
                    await context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

    }
}
