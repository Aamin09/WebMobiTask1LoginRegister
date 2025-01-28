using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly WebMobiTask1DbContext context;
        public readonly IWebHostEnvironment env;
        public LoginController(WebMobiTask1DbContext _context, IWebHostEnvironment _env, ILogger<LoginController> logger)
        {
            context = _context;
            env = _env;
            _logger = logger;
        }



        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (HttpContext.Session.GetString("UserSession") != null)
            {

                ViewBag.LoginMessage = "You are already logged in.";
               
                return RedirectToAction("Index", "Products");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Userlogin u,string returnUrl)
        {

            var data = await context.Userlogins.Where(x => x.Email == u.Email && x.Password == u.Password).FirstOrDefaultAsync();

            if (data != null)
            {
                if (!data.IsActive)
                {
                    ViewBag.Accounterror = "<script>alert('Account is inactive.')</script>";
                    return View();
                }
                // cookie for user authentication
                var claims = new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, u.Email) };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("UserSession", u.Email);
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
              
            }


            ViewBag.error = "<script>alert('Incorrect Username or Password!')</script>";
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
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
                    return RedirectToAction("Login", "Login");
                }
            }
            return View(u);
        }
    }
}
