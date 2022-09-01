using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserEditDto
{
    public string? Username { get; set; } = string.Empty;
    
    [DataType(DataType.Upload)] 
    public IFormFile File { get; set; }
    public string? Credo { get; set; } = string.Empty;
    public string? LanguageOfCommunication { get; set; } = string.Empty;
    public string? AboutMyself { get; set; } = string.Empty;
    public string? MaritalStatus { get; set; } = string.Empty;
    public string? GetAcquaintedWith { get; set; } = string.Empty;
    public string? IWantToLearn { get; set; } = string.Empty;
    public string? FavoritePlace { get; set; } = string.Empty;
    public string? MyInterests { get; set; } = string.Empty;
}