using onlinefood.Services.Interfaces;
using onlinefood.Dto.UserDtos;
using onlinefood.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using onlinefood.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Transactions;
using onlinefood.ViewModels.UserVms;

namespace onlinefood.Services;

public class UserService : IUserService
{
    private readonly FirstRunDbContext dbContext;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IEmailService emailService;

    public UserService(FirstRunDbContext dbContext, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
    {
        this.emailService = emailService;
        this.httpContextAccessor = httpContextAccessor;
        this.dbContext = dbContext;
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string enteredPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
    }


    public async Task RegisterUser(RegisterUserDto UserDto)
    {
        if (string.IsNullOrEmpty(UserDto.Username) || string.IsNullOrEmpty(UserDto.Email) || string.IsNullOrEmpty(UserDto.Password) ||
        string.IsNullOrEmpty(UserDto.ConfirmPassword))
        {
            throw new Exception("All Fields are required");
        }
        if (UserDto.Password != UserDto.ConfirmPassword)
        {
            throw new Exception("Password and Confirm Password do not match");
        }
        if (UserDto.Password.Length < 6)
        {
            throw new Exception("Password must be at least 6 characters long");
        }


        bool emailExists = await dbContext.Users.AnyAsync(u => u.Email == UserDto.Email);
        bool userNameExists = await dbContext.Users.AnyAsync(u => u.Name == UserDto.Username);

        if (userNameExists || emailExists)
        {
            throw new InvalidOperationException("Username or Email already exists");
        }

        var user = new Users
        {
            Name = UserDto.Username,
            Email = UserDto.Email,
            Password = HashPassword(UserDto.Password), // Ensure secure hashing
            Role = "User",
            IsVerified = false,
            Phone = UserDto.Phone,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.Users.AddAsync(user);

        var verificationToken = Guid.NewGuid().ToString("N").Substring(0, 6);

        var verification = new EmailVerification
        {
            UserId = user.UserId,
            User = user,
            VerificationCode = verificationToken,
            ExpiryDate = DateTime.UtcNow.AddHours(1),
            IsVerified = false
        };
        await dbContext.EmailVerifications.AddAsync(verification);
        await dbContext.SaveChangesAsync();

        try
        {
            await emailService.SendEmailAsync(user.Email, "Email Verification",
                $"<h1>Welcome {user.Name}</h1><p>Please verify your email using this code: {verificationToken}</p>");
        }
        catch (Exception ex)
        {
            throw new Exception("Error sending email: " + ex.Message);
        }
    }

    public async Task<Users> LoginUser(LoginUserDto UserDto)
    {
        if (string.IsNullOrEmpty(UserDto.Username) && string.IsNullOrEmpty(UserDto.Email))
        {
            throw new Exception("Username or Email is required");
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == UserDto.Username || u.Email == UserDto.Email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        if (!VerifyPassword(UserDto.Password, user.Password))
        {
            throw new Exception("Invalid password");
        }
        if (!user.IsVerified)
        {
            throw new Exception("Please verify your email before logging in.");
        }

        var httpContext = httpContextAccessor.HttpContext;
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new (ClaimTypes.Name, user.Name),
            new (ClaimTypes.Role, user.Role),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return user;
    }

    public async Task Logout()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new Exception("HttpContext is null");
        }
        try
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            throw new Exception("Error during sign out: " + ex.Message);
        }
    }

    public async Task<List<UserVm>> GetAllUsers()
    {
        var users = await dbContext.Users.ToListAsync();
        var userDtos = users.Select(u => new UserVm
        {
            Id = u.UserId,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            Phone = u.Phone,
            CreatedAt = u.CreatedAt,
            IsVerified = u.IsVerified,
        }).ToList();
        return userDtos;
    }

    public async Task<UserVm> GetUserById(int id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("The user does not exist.");
        }
        var vm = new UserVm
        {
            Id = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Phone = user.Phone,
            CreatedAt = user.CreatedAt,
            IsVerified = user.IsVerified,
        };
        return vm;
    }

    public async Task<ViewUserDto> GetUserByEmail(string email)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var userDto = new ViewUserDto
        {
            Id = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Phone = user.Phone,
            IsVerified = user.IsVerified,
            CreatedAt = user.CreatedAt
        };
        return userDto;
    }

    public async Task<bool> VerifyEmail(string email, string code)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var verification = await dbContext.EmailVerifications
            .FirstOrDefaultAsync(v => v.UserId == user.UserId && v.VerificationCode == code);

        if (verification == null || verification.IsVerified || verification.ExpiryDate < DateTime.UtcNow)
        {
            throw new Exception("Invalid or expired verification code");
        }

        verification.IsVerified = true;
        user.IsVerified = true;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task UpdateUser(int id, UserUpdateDto userDto)
    {
        var user = await dbContext.Users.Where(x => x.UserId == id).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Validate and update the name and email
        user.Name = userDto.Name;

        // Check if the email is already in use by another user
        if (userDto.Email != user.Email)
        {
            bool emailExists = await dbContext.Users.AnyAsync(u => u.Email == userDto.Email);
            if (emailExists)
            {
                throw new Exception("Email is already taken by another user.");
            }

            user.Email = userDto.Email;
        }

        // Update phone number if provided
        if (!string.IsNullOrEmpty(userDto.Phone))
        {
            user.Phone = userDto.Phone;
        }

        // Only update password if provided
        if (!string.IsNullOrEmpty(userDto.Password))
        {
            // Optionally, validate password strength here
            user.Password = HashPassword(userDto.Password);
        }

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var user = await dbContext.Users.Where(x => x.UserId == id).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new Exception("User not found");
        }
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    // Search for users by name 
    public async Task<IEnumerable<UserVm>> SearchUser(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return new List<UserVm>();
        }

        return await dbContext.Users
            .AsNoTracking()
            .Where(u => u.Name.Contains(searchTerm))
            .Select(u => new UserVm
            {
                Id = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                Phone = u.Phone,
                CreatedAt = u.CreatedAt,
                IsVerified = u.IsVerified,
            }).ToListAsync();
    }

    public int GetCurrentUserId()
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? httpContextAccessor.HttpContext?.User?.FindFirstValue("Id");
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new Exception("User ID claim not found.");
        }

        if (int.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        throw new Exception("Failed to parse UserId.");
    }
}
