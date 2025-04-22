using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlinefood.Data;
using onlinefood.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}