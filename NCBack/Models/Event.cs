

namespace NCBack.Models;

public enum Status
{
    Expectations,
    Accepted,
    Canceled
}



public enum StatusUserMyEvents
{
    MyEventsDeActive,
    MyEventsActive
}


public class Event
{
    public int Id { get; set; }
    public int AimOfTheMeetingId { get; set; }
    public AimOfTheMeeting? AimOfTheMeeting { get; set; }
    public int MeetingCategoryId { get; set; }
    public MeetingCategory? MeetingCategory { get; set; }
    public int? MeatingPlaceId { get; set; }
    public MeatingPlace? MeatingPlace { get; set; }
    public string? IWant { get; set; }
    public DateTime? TimeStart { get; set; }
    public DateTime? TimeFinish { get; set; }
    public DateTime CreateAdd { get; set; } = DateTime.Now;
    public int? CityId { get; set; }
    public CityList? City { get; set; }
    public int? GenderId { get; set; }
    public GenderList? Gender { get; set; }
    public int? AgeTo { get; set; }
    public int? AgeFrom { get; set; }
    public string? CaltulationType { get; set; }
    public string? CaltulationSum { get; set; }
    public List<string>? LanguageCommunication { get; set; }
    public List<string>? Interests { get; set; }
    
    /*public List<int>? MyInterestsId { get; set; }
    public List<MyInterests>? MyInterests { get; set; }
    public List<int>? MainСategoriesId { get; set; }
    public List<MainСategories>? MainСategories { get; set; }*/
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    /*public StatusUserMyEvents? StatusMyEvents { get; set; } = StatusUserMyEvents.MyEventsDeActive;*/
    public Status? Status { get; set; }
}