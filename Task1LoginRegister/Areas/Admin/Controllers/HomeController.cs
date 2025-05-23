using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Task1LoginRegister.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebMobiTask1DbContext context;
        private readonly IWebHostEnvironment env;

        public HomeController(ILogger<HomeController> logger, WebMobiTask1DbContext _context, IWebHostEnvironment _env)
        {
            _logger = logger;
            context = _context;
            env = _env;
        }

        public async Task<IActionResult> Index()
        {
            var data = await context.Userlogins.ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await context.Userlogins.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // POST: Userlogins/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var data = await context.Userlogins.FindAsync(id);
            if (data != null)
            {
                var imagePath = Path.Combine(env.WebRootPath, "Images", data.Photo);

                if (System.IO.File.Exists(imagePath))
                {
                    try
                    {
                        await Task.Delay(100);  // 100 ms delay
                        System.IO.File.Delete(imagePath);
                    }
                    catch (IOException ex)
                    {
                        return StatusCode(500, $"Error deleting image file: {ex.Message}");
                    }
                }


                context.Userlogins.Remove(data);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var user = context.Userlogins.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserLoginModels
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Gender = user.Gender,
                Photo = user.Photo,
                Password = user.Password,
                ConfirmPassword = user.Password,
                IsActive = user.IsActive,
                Role=user.Role,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserLoginModels u)
        {
            if (ModelState.IsValid)
            {
                var user = await context.Userlogins.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }


                user.FirstName = u.FirstName;
                user.LastName = u.LastName;
                user.Email = u.Email;
                user.Phone = u.Phone;
                user.Gender = u.Gender;
                user.Password = u.Password;
                user.Role = u.Role;
                if (u.Profile != null)
                {

                    // new photo file code
                    string filename = Guid.NewGuid().ToString() + "_" + u.Profile.FileName;
                    string folder = Path.Combine(env.WebRootPath, "Images");
                    string filepath = Path.Combine(folder, filename);
                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await u.Profile.CopyToAsync(fileStream);
                    }
                    // Deleting the old photo
                    if (!string.IsNullOrEmpty(user.Photo))
                    {
                        string oldFilePath = Path.Combine(env.WebRootPath, "Images", user.Photo);
                        try
                        {
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine($"Error deleting file: {ex.Message}");
                        }
                    }
                    user.Photo = filename;

                }
                else
                {
                    user.Photo = user.Photo;
                }


                await context.SaveChangesAsync();


                return RedirectToAction("Index");
            }


            return View(u);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await context.Userlogins.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;

            await context.SaveChangesAsync();

            return Json(new { isActive = user.IsActive });
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await context.Userlogins
                .Include(u => u.DeliveryAddresses) // Include delivery addresses
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }  
    }
}
