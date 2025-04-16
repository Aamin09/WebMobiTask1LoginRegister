using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Controllers
{
    public class CartController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private readonly UserService userService;

        public CartController(WebMobiTask1DbContext context, UserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = context.Carts.Where(c => c.UserId == userId && c.IsActive == true)
                .Include(p => p.Product).ThenInclude(pi => pi.ProductImages)
                .Include(p => p.Product).ThenInclude(pi => pi.Subcategory).ThenInclude(s => s.Taxes)
                .Include(c=>c.ProductVariant).ThenInclude(pv=>pv.ProductImages)
                .Include(c => c.ProductVariant).ThenInclude(pv => pv.VariantAttributeValues).ThenInclude(vav=>vav.ProductAttributeValue).ThenInclude(pav=>pav.Attribute).ToList();
            int cartCount = cartItems.Count;
            ViewBag.CartItemCount = cartCount > 0 ? cartCount.ToString() : "";
            return View(cartItems);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity,int? variantId)
        {
            var userEmail = User.Identity.Name ?? HttpContext.Session.GetString("UserSession");

            if (string.IsNullOrEmpty(userEmail))
            {
                string returnUrl = Url.Action("Details", "Home", new { id = productId });
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            var user = await context.Userlogins.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
            {
                return Unauthorized();
            }

            int userId = user.Id;

            var product = await context.Products.Include(p=>p.ProductVariants).FirstOrDefaultAsync(p=>p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            // checking if product has variants but no variant was selected
            if (product.HasVarinats && product.ProductVariants?.Any() == true && !variantId.HasValue)
            {
                // Build query string with currently selected attributes to redirect back
                var selectedAttrs = Request.Form.Keys
                    .Where(k => k.StartsWith("attr_"))
                    .ToDictionary(k => k, k => Request.Form[k].ToString());

                var queryString = string.Join("&", selectedAttrs.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

                TempData["ErrorMessage"] = "Please select a valid combination of product options.";
                return RedirectToAction("Details", "Home", new { id = productId, query = queryString });
            }

            decimal price;
            ProductVariant selectedVariant = null;

            if (variantId.HasValue)
            {
                selectedVariant = product.ProductVariants.FirstOrDefault(v => v.VariantId == variantId.Value);
                if (selectedVariant == null) return NotFound();
                
                price = selectedVariant.FinalSellingPrice;
            }
            else
            {
                price = product.CalculatedSellingPrice;
            }
            // Add these lines right before your foreach loop
            System.Diagnostics.Debug.WriteLine("Form Keys Count: " + Request.Form.Keys.Count);
            foreach (var key in Request.Form.Keys)
            {
                System.Diagnostics.Debug.WriteLine($"Key: {key}, Value: {Request.Form[key]}");
            }

            // Then continue with your existing code
            var selectedAttributes = new Dictionary<string, string>();
            foreach (var key in Request.Form.Keys.Where(k => k.StartsWith("attr_")))
            {
                var attributeName = key.Substring(5); // removing attr_ prefix
                selectedAttributes[attributeName] = Request.Form[key].ToString();
                System.Diagnostics.Debug.WriteLine($"Added to dict: {attributeName} = {Request.Form[key]}");
            }
            // searialize to json
            string attributesJsom= JsonSerializer.Serialize(selectedAttributes);
            var cartItem = await context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId && c.ProductVariantId == variantId && c.IsActive == true);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                cartItem.SelectedAttributes = attributesJsom;
            }
            else
            {
                cartItem = new Cart
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity,
                    Price = price,
                    ProductVariantId=variantId,
                    SelectedAttributes=attributesJsom
                };
                await context.Carts.AddAsync(cartItem);
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartId, int quantity)
        {
            var cartItem = await context.Carts
                .Include(c => c.Product)
                .Include(c=>c.ProductVariant)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found." });
            }

            if (quantity <= 0)
            {
                return Json(new { success = false, message = "Quantity must be greater than zero." });
            }

            int availableStock = cartItem.ProductVariant != null
                ? cartItem.ProductVariant.StockQuantity : cartItem.Product.StockQuantity;
            if(quantity > availableStock)
            {
                return Json(new
                {
                    success = false,
                    message = $"Only {availableStock} item available in stock."
                });
            }
            cartItem.Quantity = quantity;
            await context.SaveChangesAsync();

            decimal totalPrice = (cartItem.Quantity * cartItem.Price) + (cartItem.Product?.DeliveryCharge ?? 0);
            return Json(new { success = true, total = totalPrice.ToString("C") });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            var cartItem = await context.Carts.FindAsync(cartId);
            if (cartItem != null)
            {
                context.Carts.Remove(cartItem);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetCartCountAsync()
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int cartCount = context.Carts
                .Where(c => c.UserId == userId && c.IsActive == true)
                .Count(); // Count unique products

            return Json(new { count = cartCount });
        }

    }
}
