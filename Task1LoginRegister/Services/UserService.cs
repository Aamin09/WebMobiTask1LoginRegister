using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Services
{
    public class UserService
    {
        private readonly WebMobiTask1DbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(WebMobiTask1DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<int?> GetCurrentUserIdAsync()
        {
            var userEmail = httpContextAccessor.HttpContext?.User?.Identity?.Name
                            ?? httpContextAccessor.HttpContext?.Session.GetString("UserSession");

            if (string.IsNullOrEmpty(userEmail))
            {
                return null;
            }

            var user = await context.Userlogins.FirstOrDefaultAsync(u => u.Email == userEmail);
            return user?.Id;
        }

        public string GetUserName()
        {
            var email = httpContextAccessor.HttpContext?.User.Identity.IsAuthenticated == true
                ? httpContextAccessor.HttpContext.User.Identity.Name
                : httpContextAccessor.HttpContext.Session.GetString("UserSession");

            if (!string.IsNullOrEmpty(email))
            {
                var user = context.Userlogins.FirstOrDefault(x => x.Email == email);
                return user?.FirstName ?? "User";
            }

            return "Guest";
        }

    }
}
