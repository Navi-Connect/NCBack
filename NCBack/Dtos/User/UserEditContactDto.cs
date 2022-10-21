using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserEditContactDto
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public string Phone { get; set; }
}