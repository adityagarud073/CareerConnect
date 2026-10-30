using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SmartRecruitment.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    [RegularExpression(
        @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
        ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = "";

    [Required]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,20}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.")]
    public string Password { get; set; } = "";

    [Required]
    public string Role { get; set; } = "";
}