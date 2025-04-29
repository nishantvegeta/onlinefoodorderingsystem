using Microsoft.AspNetCore.Mvc;
using onlinefood.Services.Interfaces;
using onlinefood.Dto.PaymentDtos;
using onlinefood.ViewModels.PaymentVms;

namespace onlinefood.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(CreatePaymentVm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid payment data." });
            }

            var dto = new CreatePaymentDto();
            dto.OrderId = vm.OrderId;
            dto.PaymentMethod = vm.PaymentMethod;
            dto.PaymentToken = vm.PaymentToken;
            dto.Amount = vm.Amount;

            var success = await paymentService.ProcessPayment(dto);
            if (!success)
            {
                ModelState.AddModelError("", "Payment failed.");
                return View(vm);
            }

            TempData["Success"] = "Payment successful!";
            return RedirectToAction("OrderConfirmation", new { id = vm.OrderId });
        }

    }
}
