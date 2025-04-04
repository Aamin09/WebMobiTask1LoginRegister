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
        public async Task<IActionResult> AddReview(ReviewViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Store the entered values to repopulate the form
                    TempData["SavedRating"] = model.Rating;
                    TempData["SavedDescription"] = model.Description;
                    TempData["ErrorMessage"] = "Please correct the errors in your review";
                    return RedirectToAction("Details", "Home", new { id = model.ProductId });
                }

                var userId = userService.GetUserId();
                if (userId == null)
                {
                    TempData["ErrorMessage"] = "You must be logged in to submit a review";
                    return RedirectToAction("Details", "Home", new { id = model.ProductId });
                }

                bool hasPurchased = await context.OrderItems
                    .AnyAsync(oi => oi.ProductId == model.ProductId &&
                                  oi.Order.UserId == userId &&
                                  oi.Order.OrderStatus == "Delivered");

                // Uncomment this if you want to enforce purchase verification
                /*
                if (!hasPurchased)
                {
                    TempData["ErrorMessage"] = "You can only review products you have purchased";
                    return RedirectToAction("Details", "Home", new { id = model.ProductId });
                }
                */

                var existingReview = await context.Reviews
                    .FirstOrDefaultAsync(r => r.ProductId == model.ProductId && r.UserId == userId);

                if (existingReview != null)
                {
                    existingReview.Rating = model.Rating;
                    existingReview.Description = model.Description;
                    existingReview.CreatedDate = DateTime.Now;
                    existingReview.IsApproved = true;  // Changed to auto-approve updated reviews too
                }
                else
                {
                    var review = new Review
                    {
                        ProductId = model.ProductId,
                        UserId = (int)userId,
                        Rating = model.Rating,
                        Description = model.Description,
                        CreatedDate = DateTime.Now,
                        IsApproved = true  // Auto-approve new reviews
                    };
                    context.Reviews.Add(review);
                }

                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your review has been submitted successfully!";
                return RedirectToAction("Details", "Home", new { id = model.ProductId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while submitting your review";
                return RedirectToAction("Details", "Home", new { id = model.ProductId });
            }
        }
    }
}