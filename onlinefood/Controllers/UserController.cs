using Microsoft.AspNetCore.Mvc;
using onlinefood.Services.Interfaces;
using onlinefood.Dto.UserDtos;
using onlinefood.Entity;
using onlinefood.Data;
using Microsoft.EntityFrameworkCore;
using onlinefood.ViewModels;

namespace onlinefood.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly FirstRunDbContext dbContext;

        public UserController(IUserService userService, FirstRunDbContext dbContext)
        {
            this.userService = userService;
            this.dbContext = dbContext;
        }

        public ActionResult Register()
        {
            var vm = new RegisterVm();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userDto = new RegisterUserDto();
                userDto.Username = vm.Username;
                userDto.Email = vm.Email;
                userDto.Password = vm.Password;
                userDto.Role = vm.Role;
                userDto.IsVerified = vm.IsVerified;
                userDto.Phone = vm.Phone;

                await userService.RegisterUser(userDto);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        public ActionResult Login()
        {
            var vm = new LoginVm();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userDto = new LoginUserDto();
                userDto.Username = vm.Username;
                userDto.Email = vm.Email;
                userDto.Password = vm.Password;

                var user = await userService.LoginUser(userDto);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }
                else if (user.Role == "User")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user role");
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await userService.Logout();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
