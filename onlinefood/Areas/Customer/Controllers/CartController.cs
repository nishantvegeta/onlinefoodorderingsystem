using Microsoft.AspNetCore.Mvc;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.CartItemVms;
using onlinefood.Entity;
using onlinefood.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using onlinefood.Enums;

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
            return View(cartViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int foodItemId, int quantity)
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                Console.WriteLine($"UserId: {userId}");

                if (quantity <= 0)
                {
                    ModelState.AddModelError("Quantity", "Quantity must be greater than zero.");
                    return RedirectToAction("Index");
                }

                await cartService.AddToCart(userId, foodItemId, quantity);

                TempData["success"] = "Item added to cart successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while adding the item to the cart.";
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int foodItemId)
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                Console.WriteLine($"UserId: {userId}");

                await cartService.RemoveFromCart(userId, foodItemId);
                TempData["Success"] = "Item removed from cart successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while removing the item from the cart.";
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = userService.GetCurrentUserId();

                await cartService.ClearCart(userId);
                TempData["Success"] = "Cart cleared successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while clearing the cart.";
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int foodItemId, int quantity)
        {
            try
            {
                // Get the current user ID
                var userId = userService.GetCurrentUserId();

                // Validate that the quantity is greater than zero
                if (quantity <= 0)
                {
                    TempData["Error"] = "Quantity must be greater than zero.";
                    return RedirectToAction("Index");
                }

                // You can also add a maximum limit for the quantity, if required
                if (quantity > 100) // Example: Maximum quantity limit
                {
                    TempData["Error"] = "You cannot add more than 100 of this item.";
                    return RedirectToAction("Index");
                }

                // Call the cart service to update the quantity of the food item
                await cartService.UpdateQuantity(userId, foodItemId, quantity);
                TempData["Success"] = "Quantity updated successfully!"; 

                // Redirect back to the cart page
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                TempData["Error"] = "An unexpected error occurred while updating the quantity.";
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PaymentMethod paymentMethod)
        {
            Console.WriteLine($"Payment Method ------------------------------>: {paymentMethod}");
            try
            {
                var userId = userService.GetCurrentUserId();
                if (userId == 0)
                {
                    TempData["Error"] = "You must be logged in to place an order.";
                    return RedirectToAction("Login", "Auth");
                }

                var cartItems = await cartService.GetCartItems(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    TempData["Error"] = "Your cart is empty.";
                    return RedirectToAction("Index");
                }

                var totalPrice = await cartService.GetTotalPrice(userId);
                if (totalPrice <= 0)
                {
                    TempData["Error"] = "Total price must be greater than zero.";
                    return RedirectToAction("Index");
                }

                await cartService.PlaceOrder(userId, paymentMethod);
                TempData["Success"] = "Your order has been placed successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while placing the order.";
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
                TempData["Error"] = "An error occurred while retrieving the cart items.";
                return BadRequest(ex.Message);
            }
        }
    }
}
