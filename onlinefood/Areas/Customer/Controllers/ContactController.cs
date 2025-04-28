using Microsoft.AspNetCore.Mvc;
using onlinefood.Data;
using onlinefood.Dto.ContactDtos;
using onlinefood.Entity;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.ContactVms;

namespace onlinefood.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactController : Controller
    {
        private readonly IContactService contactService;
        private readonly FirstRunDbContext dbContext;
        public ContactController(IContactService contactService, FirstRunDbContext dbContext)
        {
            this.contactService = contactService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult CreateContact()
        {
            var vm = new CreateContactVm();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }
                var dto = new CreateContactDto
                {
                    UserId = vm.UserId,
                    Name = vm.Name,
                    Email = vm.Email,
                    Phone = vm.Phone,
                    Subject = vm.Subject,
                    Message = vm.Message
                };

                await contactService.CreateContact(dto);
                TempData["Success"] = "Contact created successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while creating the contact.";
                return View(vm);
            }
        }

    }
}
