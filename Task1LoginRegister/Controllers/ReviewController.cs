using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Controllers
{
    public class ReviewController : Controller
    {
        private readonly UserService userService;
        private readonly WebMobiTask1DbContext context;

        public ReviewController(UserService userService, WebMobiTask1DbContext context)
        {
            this.userService = userService;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
       public async Task<IActionResult> AddReview(int productId, int rating, string description)
        {
            if(rating < 1 || rating > 5)
            {
                ViewBag.Error = "Rating must be between 1 and 5";
            }

            if(string.IsNullOrEmpty(description))
            {
                ViewBag.Error = "Please provide a review comment";
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Home", new {id=productId});
            }

            var userId= userService.GetUserId();

            var existingReview = await context.Reviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
            if (existingReview != null)
            {
                existingReview.Rating = rating;
                existingReview.Description = description;
                existingReview.CreatedDate = DateTime.Now;
                existingReview.IsApproved = false;
                existingReview.ApprovedDate = null;
            }
            else
            {
                var review = new Review
                {
                    ProductId = productId,
                    UserId = (int)userId,
                    Rating = rating,
                    Description = description
                };
                context.Reviews.Add(review);
            }
           
            await context.SaveChangesAsync();

            ViewBag.Success = "Your review has been submitted";
            return RedirectToAction("Details","Home",new {id=productId});
        }
    }
}
