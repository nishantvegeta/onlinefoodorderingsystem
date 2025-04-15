using System;

namespace onlinefood.Dto.UserDtos;

public class ViewUserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}
