using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserEditPhotoDto
{
    [DataType(DataType.Upload)] 
    public IFormFile File { get; set; }
}