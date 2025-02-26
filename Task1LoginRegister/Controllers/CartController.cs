using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> IndexAsync()
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var cartItems = context.Carts.Where(c=>c.UserId== userId && c.IsActive == true).Include(p=>p.Product).ThenInclude(pi=>pi.ProductImages).Include(p => p.Product).ThenInclude(pi => pi.Subcategory).ThenInclude(s => s.Taxes).ToList();
            int cartCount = cartItems.Count;
            ViewBag.CartItemCount = cartCount > 0 ? cartCount.ToString() : ""; 
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userEmail = User.Identity.Name ?? HttpContext.Session.GetString("UserSession");

            if (string.IsNullOrEmpty(userEmail))
            {
                string returnUrl = Url.Action("Details", "Home", new { id = productId });
                return RedirectToAction("Login", "Login", new { returnUrl });
            }

            var user = await context.Userlogins.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
            {
                return Unauthorized();
            }

            int userId = user.Id;

            var product = await context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = await context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId && c.IsActive == true);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new Cart
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity,
                    Price = product.CalculatedSellingPrice
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
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found." });
            }

            if (quantity <= 0)
            {
                return Json(new { success = false, message = "Quantity must be greater than zero." });
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
                await  context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetCartCountAsync()
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            int cartCount = context.Carts
                .Where(c => c.UserId == userId && c.IsActive == true)
                .Count(); // Count unique products

            return Json(new { count = cartCount }); 
        }

    }
}
