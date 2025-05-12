using Microsoft.AspNetCore.Mvc;
using onlinefood.Entity;
using onlinefood.ViewModels.OrderVms;
using Microsoft.AspNetCore.Authorization;
using onlinefood.Data;
using onlinefood.Services.Interfaces;
using onlinefood.Services;
using onlinefood.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace onlinefood.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {

        private readonly IOrderService orderService;
        private readonly IUserService userService;
        private readonly ICartService cartService;

        public OrderController(IOrderService orderService, IUserService userService, ICartService cartService)
        {
            this.orderService = orderService;
            this.userService = userService;
            this.cartService = cartService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(PlaceOrderVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var userId = userService.GetCurrentUserId();

                var dto = new Dto.OrderDtos.PlaceOrderDto();
                dto.UserId = userId;
                dto.UserName = vm.UserName;
                dto.TotalAmount = vm.TotalAmount;
                dto.DeliveryAddress = vm.DeliveryAddress;
                dto.Email = vm.Email;
                dto.Phone = vm.Phone;
                dto.CartItemIds = vm.CartItemIds;
                dto.PaymentMethod = vm.PaymentMethod;

                await orderService.CreateOrder(dto, userId);

                TempData["Success"] = "Order placed successfully!";
                return RedirectToAction("MyOrders");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while placing the order.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                var orders = await orderService.GetOrdersByUserId(userId);
                if (orders == null || !orders.Any())
                {
                    ViewBag.NoOrders = "You haven't placed any orders yet.";
                }
                return View(orders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching orders.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                var orderDetails = await orderService.GetOrderById(userId, orderId);
                if (orderDetails == null)
                {
                    return NotFound();
                }
                return View(orderDetails);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching order details.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                var order = await orderService.GetOrderById(orderId, userId);
                if (order == null)
                {
                    return NotFound();
                }

                await orderService.CancelOrder(orderId, userId);
                TempData["Success"] = "Order cancelled successfully!";
                return RedirectToAction("MyOrders");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while cancelling the order.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                var cartItems = await cartService.GetCartItems(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    TempData["Error"] = "Your cart is empty!";
                    return RedirectToAction("Index", "Home");
                }

                var vm = new PlaceOrderVm
                {
                    UserId = userId,
                    CartItemIds = cartItems.Select(x => x.CartItemId).ToList(),
                    TotalAmount = cartItems.Sum(x => x.Price * x.Quantity),
                    PaymentMethods = Enum.GetValues(typeof(PaymentMethod))
                        .Cast<PaymentMethod>()
                        .Select(e => new SelectListItem
                        {
                            Value = e.ToString(),
                            Text = e.ToString()
                        })
                        .ToList()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching checkout details.");
                return BadRequest(ex.Message);
            }
        }
    }
}
