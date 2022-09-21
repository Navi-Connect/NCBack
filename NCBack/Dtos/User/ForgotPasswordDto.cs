using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NCBack.Dtos.User;

public class ForgotPasswordDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}