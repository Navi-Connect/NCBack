using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserRegisterDto
{
    [Required] 
    public string City { get; set; }
    [Required]
    public string PhoneNumber { get; set; }  
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Fullname { get; set; }
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
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