namespace NCBack.Models;

public class User
{
    public int Id { get; set; }
    public string City { get; set; }
    public string? Region { get; set; } = string.Empty;
    public string PhoneNumber  { get; set; }
    public string Email { get; set; }
    public int? Code { get; set; } 
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string Lastname { get; set; } 
    public string? SurName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string AvatarPath { get; set; }
    public string? Credo { get; set; } = string.Empty;
    public string? LanguageOfCommunication { get; set; } = string.Empty;
    public string? AboutMyself { get; set; } = string.Empty;
    public string? MaritalStatus { get; set; } = string.Empty;
    public string? GetAcquaintedWith { get; set; } = string.Empty;
    public string? IWantToLearn { get; set; } = string.Empty;
    public string? FavoritePlace { get; set; } = string.Empty;
    public string? MyInterests { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; }
    public string? Token { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}