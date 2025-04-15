using System;
using onlinefood.Dto.UserDtos;

namespace onlinefood.Services.Interfaces;

public interface IUserService
{
    Task RegisterUser(RegisterUserDto UserDto);
    Task LoginUser(LoginUserDto UserDto);
    Task Logout();
    Task<ViewUserDto> GetUserById(int id);
    Task<List<ViewUserDto>> GetAllUsers();
}   
