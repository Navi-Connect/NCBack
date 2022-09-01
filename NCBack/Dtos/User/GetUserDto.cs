namespace NCBack.Dtos.User;

public class GetUserDto
{
    public string City { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    public string? SurName { get; set; } = string.Empty;
    public string AvatarPath { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string? Token { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}