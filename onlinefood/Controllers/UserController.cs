using Microsoft.AspNetCore.Mvc;
using onlinefood.Services.Interfaces;
using onlinefood.Dto.UserDtos;
using onlinefood.Entity;
using onlinefood.Data;
using Microsoft.EntityFrameworkCore;
using onlinefood.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace onlinefood.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly FirstRunDbContext dbContext;

        public UserController(IUserService userService, FirstRunDbContext dbContext)
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
                return RedirectToAction("VerifyEmail", "User");
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
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(string email, string code)
        {
            try
            {
                var user = await userService.GetUserByEmail(email);

                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View();
                }

                var verification = await dbContext.EmailVerifications
                    .FirstOrDefaultAsync(v => v.UserId == user.Id && v.VerificationCode == code);

                if (verification == null || verification.ExpiryDate < DateTime.UtcNow)
                {
                    ModelState.AddModelError("", "Invalid or expired verification code");
                    return View();
                }

                verification.IsVerified = true;
                user.IsVerified = true;
                await dbContext.SaveChangesAsync();
                TempData["Success"] = "Email verified successfully. You can now login.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}
