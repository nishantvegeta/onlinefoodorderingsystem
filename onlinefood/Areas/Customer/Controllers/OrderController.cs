using Microsoft.AspNetCore.Mvc;
using onlinefood.Entity;
using onlinefood.ViewModels.OrderVms;
using Microsoft.AspNetCore.Authorization;
using onlinefood.Data;
using onlinefood.Services.Interfaces;
using onlinefood.Services;
using onlinefood.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace onlinefood.Areas.Customer.Controllers
{
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var userId = userService.GetCurrentUserId();
            var orders = await orderService.GetOrdersByUserId(userId);
            return View(orders);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var userId = userService.GetCurrentUserId();
            var order = await orderService.GetOrderById(orderId, userId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int orderId)
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Checkout()
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
    }
}
