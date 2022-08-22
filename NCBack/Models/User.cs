using Microsoft.AspNetCore.Identity;

namespace NCBack.Models;

public class User 
{
    public Guid Id { get; set; }
    public string City { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    public string? SurName { get; set; } = string.Empty;
    // public string Phone { get; set; }
    public string AvatarPath { get; set; }
    public byte[] PasswordHash { get; set; }  = new byte[32];
    public byte[] PasswordSalt { get; set; }  = new byte[32];
    public string? Token { get; set; } = null;
}