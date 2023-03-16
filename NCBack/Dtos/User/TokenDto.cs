using NCBack.Models;

namespace NCBack.Dtos.User;

public class TokenDto
{
    /*public int UserId { get; set; }*/
    /*public int? CityId { get; set; }
    public CityList? City { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string? FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string AvatarPath { get; set; }
    public int? GenderId { get; set; }
    public GenderList? Gender { get; set; }*/
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    /*public string? DeviceId { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;*/
}