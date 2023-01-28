

namespace NCBack.Models;

public enum Status
{
    Expectations,
    Accepted,
    Canceled
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
    public string? IWant { get; set; } = null;
    public DateTime? TimeStart { get; set; }
    public DateTime? TimeFinish { get; set; }
    public DateTime CreateAdd { get; set; } = DateTime.Now;
    public int? CityId { get; set; }
    public CityList? City { get; set; }
    public int? GenderId { get; set; }
    public GenderList? Gender { get; set; }
    public int? AgeTo { get; set; } = null;
    public int? AgeFrom { get; set; } = null;
    public string? CaltulationType { get; set; } = null;
    public string? CaltulationSum { get; set; } = null;
    public List<string>? LanguageCommunication { get; set; } = null;
    public List<string>? Interests { get; set; }= null;
    
    /*public List<int>? MyInterestsId { get; set; }
    public List<MyInterests>? MyInterests { get; set; }
    public List<int>? MainСategoriesId { get; set; }
    public List<MainСategories>? MainСategories { get; set; }*/
    public double? Latitude { get; set; }= null;
    public double? Longitude { get; set; } = null;
    public int UserId { get; set; }
    public User User { get; set; }
    /*public StatusUserMyEvents? StatusMyEvents { get; set; } = StatusUserMyEvents.MyEventsDeActive;*/
    public Status? Status { get; set; }
}