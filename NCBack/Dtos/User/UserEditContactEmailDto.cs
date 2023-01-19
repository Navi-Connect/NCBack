using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserEditContactEmailDto
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}