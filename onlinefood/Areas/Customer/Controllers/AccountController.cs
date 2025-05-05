using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using onlinefood.Data;
using onlinefood.Dto.UserDtos;
using onlinefood.Entity;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.UserVms;

namespace onlinefood.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly FirstRunDbContext dbContext;
        public AccountController(IUserService userService, FirstRunDbContext dbContext)
        {
            this.userService = userService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewProfile()
        {
            try
            {
                var userId = userService.GetCurrentUserId();
                var user = await userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var vm = new ProfileVm();
                vm.UserId = user.Id;
                vm.Name = user.Name;
                vm.Email = user.Email;
                vm.Role = user.Role;
                vm.IsVerified = user.IsVerified;

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var userId = userService.GetCurrentUserId();
            var user = await userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var vm = new ProfileVm
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsVerified = user.IsVerified
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(ProfileVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var userId = userService.GetCurrentUserId();
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(vm.Password) && userService.VerifyPassword(vm.Password, user.Password))
                {
                    ModelState.AddModelError("Password", "The new password cannot be the same as the current password.");
                    return View(vm); // Return with error if new password is the same as the current password
                }

                var dto = new UserUpdateDto();
                dto.Name = vm.Name;
                dto.Email = vm.Email;
                dto.IsVerified = vm.IsVerified;

                if (!string.IsNullOrEmpty(vm.Password) && vm.Password == vm.ConfirmPassword)
                {
                    // Hash the new password and update
                    user.Password = userService.HashPassword(vm.Password);
                }
                else if (!string.IsNullOrEmpty(vm.Password) && vm.Password != vm.ConfirmPassword)
                {
                    ModelState.AddModelError("Password", "Password and confirmation password do not match.");
                    return View(vm); // Return with error if passwords don't match
                }

                await userService.UpdateUser(userId, dto);

                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction("ViewProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the profile: " + ex.Message);
                return View(vm);
            }
        }
    }
}
