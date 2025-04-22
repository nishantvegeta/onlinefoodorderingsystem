using Microsoft.AspNetCore.Mvc;
using onlinefood.Data;
using onlinefood.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.OrderVms;

namespace onlinefood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {

        private readonly FirstRunDbContext dbContext;
        private readonly IOrderService orderService;
        public OrdersController(FirstRunDbContext dbContext, IOrderService orderService)
        {
            this.orderService = orderService;
            this.dbContext = dbContext;
        }

        // view orders
        public async Task<IActionResult> Index()
        {
            var orders = await orderService.GetAllOrders();
            return View(orders);
        }

        // view order details
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await orderService.GetOrderById(id);
                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update order status
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var order = await dbContext.Orders.Where(x => x.OrderId == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }
                var vm = new UpdateOrderStatusVm();
                vm.Status = order.Status;
                vm.OrderId = order.OrderId;

                return View(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateOrderStatusVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var order = await dbContext.Orders.Where(x => x.OrderId == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }

                var dto = new Dto.OrderDtos.UpdateOrderStatusDto();
                dto.Status = vm.Status;
                dto.OrderId = vm.OrderId;

                await orderService.UpdateOrderStatus(dto);
                TempData["success"] = "Order status updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }


    }
}
