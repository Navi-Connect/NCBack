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
    [Required]
    public string SurName { get; set; }
    //public string Phone { get; set; }
    [Required]
    [DataType(DataType.Upload)]
    public IFormFile File { get; set; }
    [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters, dude!")]
    public string Password { get; set; }
    [Required, Compare("Password")]
    public string ConfirmPassword { get; set; } 
}