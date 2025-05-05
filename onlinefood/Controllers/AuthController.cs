using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlinefood.Data;
using onlinefood.Dto.UserDtos;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.AccountVms;

namespace onlinefood.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IUserService userService;
        private readonly FirstRunDbContext dbContext;

        public AuthController(IUserService userService, FirstRunDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.userService = userService;
        }

        public IActionResult Register()
        {
            var vm = new RegisterVm();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var userDto = new RegisterUserDto();
                userDto.Username = vm.Username;
                userDto.Email = vm.Email;
                userDto.Password = vm.Password;
                userDto.ConfirmPassword = vm.ConfirmPassword;
                userDto.Role = vm.Role;
                userDto.IsVerified = vm.IsVerified;

                await userService.RegisterUser(userDto);
                TempData["Success"] = "Registration successful. Please login to continue.";
                return RedirectToAction("VerifyEmail", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        public IActionResult Login()
        {
            var vm = new LoginVm();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var userDto = new LoginUserDto();
                userDto.Username = vm.Username;
                userDto.Email = vm.Email;
                userDto.Password = vm.Password;

                var user = await userService.LoginUser(userDto);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(vm);
                }

                Console.WriteLine($"Logged in user role: {user.Role}");

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.Role == "User")
                {
                    return RedirectToAction("Index", "Customer");
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
        [Route("user/{id:int}")]
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
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            var vm = new VerifyEmailVm();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var isVerified = await userService.VerifyEmail(vm.Email, vm.Code);
                if (isVerified)
                {
                    TempData["Success"] = "Email verified successfully.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid verification code.");
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }
    }
}
