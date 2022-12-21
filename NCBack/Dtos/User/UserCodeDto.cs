using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserCodeDto
{
    [Required] 
    public int VerificationCode { get; set; }
    
}