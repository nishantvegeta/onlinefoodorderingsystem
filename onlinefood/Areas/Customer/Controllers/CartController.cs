using Microsoft.AspNetCore.Mvc;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.CartItemVms;
using onlinefood.Entity;
using onlinefood.Data;
using Microsoft.EntityFrameworkCore;

namespace onlinefood.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {

        private readonly ICartService cartService;
        private readonly IUserService userService;
        public CartController(ICartService cartService, IUserService userService)
        {
            this.cartService = cartService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = await cartService.GetCartItems();
            var totalPrice = await cartService.GetTotalPrice();

            var cartViewModel = new CartVm
            {
                CartItems = cartItems,
                TotalPrice = totalPrice
            };
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int foodItemId, int quantity)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Quantity must be greater than zero.");
                return RedirectToAction("Index");
            }
            var userId = userService.GetCurrentUserId();

            await cartService.AddToCart(userId, foodItemId, quantity);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int foodItemId)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            await cartService.RemoveFromCart(foodItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            await cartService.ClearCart();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int foodItemId, int quantity)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Quantity must be greater than zero.");
                return RedirectToAction("Index");
            }

            await cartService.UpdateQuantity(foodItemId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = userService.GetCurrentUserId();
            await cartService.PlaceOrder(userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cartItems = await cartService.GetCartItems();
            return Json(new { cartItems = cartItems });
        }
    }
}
