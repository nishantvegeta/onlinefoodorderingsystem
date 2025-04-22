using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace onlinefood.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

    }
}
