using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly WebMobiTask1DbContext context;
        public readonly IWebHostEnvironment env;
        private readonly UserService userService;

        public AccountController(WebMobiTask1DbContext _context, IWebHostEnvironment _env,UserService userService, ILogger<AccountController> logger)
        {
            context = _context;
            env = _env;
            this.userService = userService;
            _logger = logger;
        }



        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.LoginMessage = "You are already logged in.";
                return RedirectToAction("Index", "Home");
            }


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Userlogin u, string returnUrl)
        {

            var data = await context.Userlogins.Where(x => x.Email == u.Email && x.Password == u.Password && u.Role == "User").FirstOrDefaultAsync();

            if (data != null)
            {
                if (!data.IsActive)
                {
                    ViewBag.Accounterror = "<script>alert('Account is inactive.')</script>";
                    return View();
                }
                // cookie for user authentication
                var claims = new List<System.Security.Claims.Claim>
                { 
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, u.Email),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, data.Role)
                };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("UserSession", u.Email);
                HttpContext.Session.SetString("UserRole", data.Role);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                if (data.Role == "Admin")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (data.Role == "User")
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");
            }
            ViewBag.error = "<script>alert('Incorrect Username or Password!')</script>";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // Clear session
            Response.Cookies.Delete(".AspNetCore.Cookies"); // Ensure authentication cookie is deleted
            return RedirectToAction("Login", "Account");
        }


        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserLoginModels u)
        {

            if (ModelState.IsValid)
            {
                string filename = "";
                if (u.Profile != null)
                {
                    string folder = Path.Combine(env.WebRootPath, "Images");
                    filename = Guid.NewGuid().ToString() + "_" + u.Profile.FileName;
                    string filepath = Path.Combine(folder, filename);
                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await u.Profile.CopyToAsync(fileStream);
                    }
                    Userlogin userlogin = new Userlogin()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Phone = u.Phone,
                        Photo = filename,
                        Gender = u.Gender,
                        Password = u.Password,
                        IsActive = u.IsActive,
                    };
                    await context.Userlogins.AddAsync(userlogin);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(u);
        }

        public async Task<IActionResult> UserOrders()
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.ProductImages)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}
