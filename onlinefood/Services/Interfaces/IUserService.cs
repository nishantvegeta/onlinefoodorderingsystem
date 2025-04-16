using System;
using onlinefood.Dto.UserDtos;
using onlinefood.Entity;

namespace onlinefood.Services.Interfaces;

public interface IUserService
{
    Task RegisterUser(RegisterUserDto UserDto);
    Task<Users> LoginUser(LoginUserDto UserDto);
    Task Logout();
    Task<ViewUserDto> GetUserById(int id);
    Task<List<ViewUserDto>> GetAllUsers();
}   
