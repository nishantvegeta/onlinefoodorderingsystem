using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using onlinefood.Data;

namespace onlinefood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProfileController : Controller
    {

        private readonly FirstRunDbContext dbContext;
        public ProfileController(FirstRunDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            var admin = dbContext.Users.FirstOrDefault(u => u.Role == "Admin");

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

    }
}
