using Microsoft.AspNetCore.Mvc;

namespace onlinefood.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        // GET: CartController
        public ActionResult Index()
        {
            return View();
        }

    }
}
