using System.ComponentModel.DataAnnotations;
using api_teszt.Enums;
using api_teszt.Models;

namespace api_teszt.Dtos;

public class CreateUserDto
{
    [Required]
    [MaxLength(50)]
    [MinLength(2)]
    public string first_name { get; set; }
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string last_name { get; set; }
    [Required]
    [EmailAddress]
    public string email { get; set; }
    public string bio { get; set; }
    public List<Tag> tags { get; set; } = new();
}
public class UserDto
{

    public UserDto(User user)
    {
        bio = user.bio;
        first_name = user.first_name;
        last_name = user.last_name;
        email = user.email;
    }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string bio { get; set; }
}


public class UpdateUserDto
{
    #nullable enable
    public string? first_name { get; set; }
    public string? last_name { get; set; }
    public string? email { get; set; }
    public string? bio { get; set; }
}