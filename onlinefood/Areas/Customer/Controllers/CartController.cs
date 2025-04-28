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
            var userId = userService.GetCurrentUserId();
            var cartItems = await cartService.GetCartItems(userId);
            var totalPrice = await cartService.GetTotalPrice(userId);

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
            try
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while adding the item to the cart.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int foodItemId)
        {
            try
            {
                var userId = userService.GetCurrentUserId();

                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await cartService.RemoveFromCart(userId, foodItemId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while removing the item from the cart.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await cartService.ClearCart(userId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while clearing the cart.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int foodItemId, int quantity)
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (quantity <= 0)
                {
                    ModelState.AddModelError("Quantity", "Quantity must be greater than zero.");
                    return RedirectToAction("Index");
                }

                await cartService.UpdateQuantity(userId, foodItemId, quantity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the quantity.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                if (userId == 0)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var cartItems = await cartService.GetCartItems(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    ModelState.AddModelError("", "Your cart is empty.");
                    return RedirectToAction("Index");
                }

                var totalPrice = await cartService.GetTotalPrice(userId);
                if (totalPrice <= 0)
                {
                    ModelState.AddModelError("", "Total price must be greater than zero.");
                    return RedirectToAction("Index");
                }

                await cartService.PlaceOrder(userId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while placing the order.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var cartItems = await cartService.GetCartItems(userId);
                return Json(new { cartItems = cartItems });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving the cart items.");
                return BadRequest(ex.Message);
            }
        }
    }
}
