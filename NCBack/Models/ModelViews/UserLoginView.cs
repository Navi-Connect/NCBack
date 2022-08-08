using System.ComponentModel.DataAnnotations;

namespace NCBack.Models.ModelViews;

public class UserLoginView
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}