using System;
using onlinefood.Services.Interfaces;
using onlinefood.Dto.UserDtos;
using onlinefood.Data;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using onlinefood.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace onlinefood.Services;

public class UserService : IUserService
{
    private readonly FirstRunDbContext dbContext;
    private readonly IHttpContextAccessor httpContextAccessor;
    public UserService(FirstRunDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.dbContext = dbContext;
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string enteredPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
    }


    public async Task RegisterUser(RegisterUserDto UserDto)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        bool userNameExists = await dbContext.Users.AnyAsync(u => u.Name == UserDto.Username);
        bool emailExists = await dbContext.Users.AnyAsync(u => u.Email == UserDto.Email);

        if (userNameExists || emailExists)
        {
            throw new Exception("Username or Email already exists");
        }

        var user = new Users
        {
            Name = UserDto.Username,
            Email = UserDto.Email,
            Password = HashPassword(UserDto.Password),
            Role = UserDto.Role,
            IsVerified = UserDto.IsVerified,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        transaction.Complete();
    }

    public async Task<Users> LoginUser(LoginUserDto UserDto)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == UserDto.Username || u.Email == UserDto.Email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        if (!VerifyPassword(UserDto.Password, user.Password))
        {
            throw new Exception("Invalid password");
        }

        var httpContext = httpContextAccessor.HttpContext;
        var claims = new List<Claim>
        {
            new ("Id", user.UserId.ToString()),
            new (ClaimTypes.Name, user.Name),
            new (ClaimTypes.Role, user.Role),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity));

        return user;
    }

    public async Task Logout()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new Exception("HttpContext is null");
        }
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<List<ViewUserDto>> GetAllUsers()
    {
        var users = await dbContext.Users.ToListAsync();
        var userDtos = users.Select(u => new ViewUserDto
        {
            Id = u.UserId,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            IsVerified = u.IsVerified,
            CreatedAt = u.CreatedAt
        }).ToList();
        return userDtos;
    }

    public async Task<ViewUserDto> GetUserById(int id)
    {
        var user = await dbContext.Users.FindAsync(id);
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
            IsVerified = user.IsVerified,
            CreatedAt = user.CreatedAt
        };
        return userDto;
    }
}
