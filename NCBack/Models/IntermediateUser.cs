namespace NCBack.Models;

public class IntermediateUser
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public CityList City { get; set; }
    public string? PhoneNumber  { get; set; }
    public string Email { get; set; }
    public int? Code { get; set; } 
    public string Username { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int GenderId { get; set; }
    public GenderList Gender { get; set; }
    public string AvatarPath { get; set; }
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}