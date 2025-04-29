using Microsoft.AspNetCore.Mvc;
using onlinefood.Services.Interfaces;
using onlinefood.Dto.PaymentDtos;
using onlinefood.ViewModels.PaymentVms;
using Microsoft.AspNetCore.Authorization;

namespace onlinefood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        // View all payments
        public async Task<IActionResult> Index()
        {
            var payments = await paymentService.GetAllPayments();
            return View(payments); // You’ll bind this to a view
        }

        public async Task<IActionResult> Details(int id)
        {
            var payment = await paymentService.GetPaymentById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment); // You’ll bind this to a view
        }
    }
}
