using Microsoft.AspNetCore.Mvc;
using onlinefood.Data;
using onlinefood.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.ContactVms;

namespace onlinefood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactsContoller : Controller
    {
        private readonly FirstRunDbContext dbContext;
        private readonly IContactService contactService;
        public ContactsContoller(FirstRunDbContext dbContext, IContactService contactService)
        {
            this.contactService = contactService;
            this.dbContext = dbContext;
        }

        // view contacts
        public async Task<IActionResult> Index()
        {
            var contacts = await contactService.GetAllContacts();
            return View(contacts);
        }

        // update contact status
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                var contact = await dbContext.Contacts.Where(x => x.ContactId == id).FirstOrDefaultAsync();
                if (contact == null)
                {
                    return NotFound();
                }
                var vm = new UpdateContactVm();
                vm.IsResolved = contact.IsResolved;
                vm.ContactId = contact.ContactId;

                return View(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, UpdateContactVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var contact = await dbContext.Contacts.Where(x => x.ContactId == vm.ContactId).FirstOrDefaultAsync();
                if (contact == null)
                {
                    return NotFound();
                }

                var dto = new Dto.ContactDtos.UpdateContactDto();
                dto.ContactId = vm.ContactId;
                dto.IsResolved = vm.IsResolved;

                await contactService.UpdateContactStatus(dto);
                if (vm.IsResolved)
                {
                    TempData["Success"] = "Contact status updated successfully";
                }
                else
                {
                    TempData["Error"] = "Contact status not updated";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
