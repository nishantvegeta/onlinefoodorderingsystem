using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace onlinefood.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

    }
}
