using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlinefood.Data;
using onlinefood.Entity;

namespace onlinefood.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly FirstRunDbContext dbContext;

        public HomeController(FirstRunDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [AllowAnonymous]
        // GET: HomeController
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
    }
}