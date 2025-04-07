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
            TempData["SavedRating"] = model.Rating;
            TempData["SavedDescription"] = model.Description;

            if(model.Rating < 1  || model.Rating > 5)
            {
                ModelState.AddModelError("Rating", "Please select a rating between 1 and 5");
            }

            if(!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["ErrorMessage"] = "Please correct the errors in your review";
                return RedirectToAction("Details","Home", new {id=model.ProductId});
            }

            var userId = userService.GetUserId();
            if(userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to submit a review";
                return RedirectToAction("Details", "Home", new { id = model.ProductId });
            }

            bool hasPurchased = await context.OrderItems.AnyAsync(oi => oi.ProductId == model.ProductId &&
                            oi.Order.UserId == userId && oi.Order.OrderStatus == "Delivered");

            if (!hasPurchased)
            {
                TempData["ErrorMessage"] = "You can only review products you have purchased as well as deliverd to your door";
                return RedirectToAction("Details", "Home", new { id = model.ProductId });
            }

            var exisitngReview = await context.Reviews.FirstOrDefaultAsync(r => r.ProductId == model.ProductId && r.UserId == userId);
            if(exisitngReview != null)
            {
                exisitngReview.Rating = model.Rating;
                exisitngReview.Description=model.Description;
                exisitngReview.CreatedDate=DateTime.Now;
                exisitngReview.IsApproved=true;
            }
            else
            {
                var review = new Review
                {
                    Rating = model.Rating,
                    Description = model.Description,
                    ProductId = model.ProductId,
                    UserId = (int)userId,
                    CreatedDate = DateTime.Now,
                    IsApproved=true
                };   
                context.Reviews.Add(review);
            }
            await context.SaveChangesAsync();

            TempData.Remove("SavedRating");
            TempData.Remove("SavedDescription");
            TempData["SuccessMessage"]="Your review has been submitted successfully!";
            return RedirectToAction("Details", "Home", new {id=model.ProductId});

        }
    }
}