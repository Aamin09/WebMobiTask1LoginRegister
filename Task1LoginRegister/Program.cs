using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Task1LoginRegister.Interfaces;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var provider = builder.Services.BuildServiceProvider();
var config=provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<WebMobiTask1DbContext>(x => x.UseSqlServer(config.GetConnectionString("dbcs")));

// Add this to your Program.cs or Startup.cs
var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ProductsImage");
if (!Directory.Exists(uploadDirectory))
{
    Directory.CreateDirectory(uploadDirectory);
}
// for authentication to access
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";

        // Expire cookie when browser is closed
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Optional: Set a timeout
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.Expiration = null;
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserService>();

// for razorpay payments
builder.Services.AddScoped<RazorPayService>();
builder.Services.AddScoped<PdfReportService>();
builder.Services.AddScoped<FinancialReportingService>();
builder.Services.AddScoped<DateRangeService>();
builder.Services.AddScoped<IImageService,ImageService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true; 
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();
