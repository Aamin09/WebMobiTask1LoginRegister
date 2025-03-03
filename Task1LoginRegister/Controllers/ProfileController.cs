using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly WebMobiTask1DbContext _context;
        private readonly UserService userService;
        private readonly IWebHostEnvironment env;

        public ProfileController(WebMobiTask1DbContext context, UserService userService, IWebHostEnvironment _env)
        {
            _context = context;
            this.userService = userService;
            env = _env;
        }
        public async Task<IActionResult> AddAddress()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(DeliveryAddress deliveryAddress)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if(userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            deliveryAddress.UserId = Convert.ToInt32(userId);
            var data=await _context.DeliveryAddresses.AddAsync(deliveryAddress);
            await _context.SaveChangesAsync();
            if (deliveryAddress.IsDefault == true)
            {
                var defaultAddress = await _context.DeliveryAddresses
            .Where(a => a.UserId == deliveryAddress.UserId && a.AddressId != deliveryAddress.AddressId).ToListAsync();

                foreach (var add in defaultAddress)
                {
                    add.IsDefault = false;
                }
                _context.UpdateRange(defaultAddress);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAddress(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.DeliveryAddresses.FindAsync(id);
            if(address != null)
            {
                _context.DeliveryAddresses.Remove(address);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile", "Account");
        }

       
        public async Task<IActionResult> EditAddress(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.DeliveryAddresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(int id,DeliveryAddress address)
        {
            if (id != address.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(address.IsDefault == true)
                    {
                        var defaultAddress = await _context.DeliveryAddresses
                            .Where(a=>a.UserId == address.UserId && a.AddressId != address.AddressId).ToListAsync();

                        foreach (var add in defaultAddress)
                        {
                            add.IsDefault = false;
                        }
                        _context.UpdateRange(defaultAddress);
                       
                    }
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!addressExists(address.AddressId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MyProfile","Account");
            }
            return View(address);
        }

        public IActionResult EditProfile(int id)
        {
            var user = _context.Userlogins.FirstOrDefault(x => x.Id == id);
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
                IsActive = user.IsActive
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(int id, UserLoginModels u)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Userlogins.FindAsync(id);
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


                await _context.SaveChangesAsync();


                return RedirectToAction("MyProfile","Account");
            }


            return View(u);
        }
        private bool addressExists(int id)
        {
            return _context.DeliveryAddresses.Any(e => e.AddressId == id);
        }
    }
}
