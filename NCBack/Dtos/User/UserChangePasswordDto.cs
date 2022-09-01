using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserChangePasswordDto
{
    [Required]
    public string? CurrentPassword { get; set; } = string.Empty;
    [Required]
    public string NewPassword { get; set; } = string.Empty;
    [Required]
    [Compare("NewPassword")]
    public string PasswordConfirm { get; set; } = string.Empty;
}