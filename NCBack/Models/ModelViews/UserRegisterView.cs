using System.ComponentModel.DataAnnotations;

namespace NCBack.Models.ModelViews;

public class UserRegisterView
{
    [Required] 
    public string City { get; set; } 
    [Required] 
    public string Username { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string Lastname { get; set; }
    public string SurName { get; set; } = string.Empty;
    //public string Phone { get; set; }
    [Required]
    [DataType(DataType.Upload)]
    public IFormFile File { get; set; }
    [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters, dude!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required, Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } 
}