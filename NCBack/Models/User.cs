using Microsoft.AspNetCore.Mvc;

namespace NCBack.Models;

/*public enum StatusUserRequest
{
    Empty,
    Expectation,
    Accepted,
}*/
public class User
{
    public int Id { get; set; }
    public int? CityId { get; set; }
    public CityList? City { get; set; }
    public string? PhoneNumber  { get; set; }
    public string Email { get; set; }
    public int? Code { get; set; } 
    public string Username { get; set; }
    public string? FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; } 
    public string AvatarPath { get; set; }
    public string? CredoAboutMyself { get; set; } = null;
    public List<string>? LanguageOfCommunication { get; set; } = null;
    public string? Nationality { get; set; } = null;
    public int? GenderId { get; set; }
    public GenderList? Gender { get; set; }
    public string? MaritalStatus { get; set; } = null;
    public string? Сhildren { get; set; } = null;
    public string? GetAcquaintedWith { get; set; } = null;
    public string? MeetFor { get; set; } = null;
    public int? From { get; set; } = null;
    public int? To { get; set; } = null;
    public string? IWantToLearn { get; set; } = null;
    /*public int? MeetingCategoryId { get; set; }
    public MeetingCategory? MeetingCategory { get; set; }
    public int? MeatingPlaceId { get; set; }
    public MeatingPlace? MeatingPlace { get; set; }*/
    public List<string>? PreferredPlaces { get; set; } = null;
    public List<string>? Interests { get; set; } = null;
    /*public List<int>? MyInterestsId { get; set; }
    public List<MyInterests>? MyInterests { get; set; }
    public List<int>? MainСategoriesId { get; set; }
    public List<MainСategories>? MainСategories { get; set; }*/
    public string? Profession { get; set; } = null;
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; }
    public string? DeviceId { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}