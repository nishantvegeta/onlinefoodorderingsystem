using Microsoft.AspNetCore.Mvc;
using onlinefood.Entity;
using onlinefood.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.UserVms;

namespace onlinefood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly FirstRunDbContext dbContext;
        private readonly IUserService userService;
        public AdminUsersController(FirstRunDbContext dbContext, IUserService userService)
        {
            this.userService = userService;
            this.dbContext = dbContext;
        }

        // search user
        public async Task<IActionResult> Search(string searchTerm)
        {
            var users = await userService.SearchUser(searchTerm); // Await the Task to get the result
            if (users == null || !users.Any()) // Now you can use .Any() on the collection
            {
                return NotFound("No users found");
            }

            return View(users); // Return the users to the view
        }

        // GET: AdminUsersController
        public async Task<IActionResult> Index()
        {
            var users = await userService.GetAllUsers();
            return View(users);
        }

        public IActionResult Update(int id)
        {
            try
            {
                var user = dbContext.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }
                var vm = new UpdateUserVm();
                vm.Name = user.Name;
                vm.Email = user.Email;
                vm.Password = user.Password;

                return View(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateUserVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }
                var user = await dbContext.Users.Where(u => u.UserId == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound();
                }

                var dto = new Dto.UserDtos.UserUpdateDto();
                dto.Name = vm.Name;
                dto.Email = vm.Email;
                dto.Password = vm.Password;

                await userService.UpdateUser(id, dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await userService.DeleteUser(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
