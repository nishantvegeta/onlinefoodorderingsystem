using System;
using onlinefood.Dto.UserDtos;
using onlinefood.Entity;
using onlinefood.ViewModels.UserVms;

namespace onlinefood.Services.Interfaces;

public interface IUserService
{
    string HashPassword(string password);
    bool VerifyPassword(string enteredPassword, string hashedPassword);
    Task RegisterUser(RegisterUserDto UserDto);
    Task<Users> LoginUser(LoginUserDto UserDto);
    Task Logout();
    Task<UserVm> GetUserById(int Id);
    Task<List<UserVm>> GetAllUsers();
    Task<ViewUserDto> GetUserByEmail(string email);
    Task<bool> VerifyEmail(string email, string code);
    Task UpdateUser(int id, UserUpdateDto userUpdateDto);
    Task DeleteUser(int id);
    Task<IEnumerable<UserVm>> SearchUser(string searchTerm);
    int GetCurrentUserId();
}   
