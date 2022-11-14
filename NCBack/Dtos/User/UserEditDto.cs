using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserEditDto
{
    [DataType(DataType.Upload)] 
    public IFormFile File { get; set; }
    public string? CredoAboutMyself { get; set; } = string.Empty;
    public string? LanguageOfCommunication { get; set; } = string.Empty;
    public string? Nationality { get; set; } = string.Empty;
    public string? Gender { get; set; } = string.Empty;
    public string? MaritalStatus { get; set; } = string.Empty;
    public string? GetAcquaintedWith { get; set; } = string.Empty;
    public string? MeetFor { get; set; } = string.Empty;
    public int? From { get; set; } = null;
    public int? To { get; set; } = null;
    public string? IWantToLearn { get; set; } = string.Empty;
    public string? FavoritePlace { get; set; } = string.Empty;
    public string? MyInterests { get; set; } = string.Empty;
    public string? Profession { get; set; } = string.Empty;
}