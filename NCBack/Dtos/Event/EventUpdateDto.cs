using System.ComponentModel.DataAnnotations.Schema;

namespace NCBack.Dtos.Event;

using Models;

public class EventUpdateDto
{
    public int AimOfTheMeetingId { get; set; }
    public int MeetingCategoryId { get; set; }
    public int MeatingPlaceId { get; set; }
    public string? IWant { get; set; } = null;
    public DateTime TimeStart { get; set; }
    public DateTime TimeFinish { get; set; }
    public int? CityId { get; set; }
    public int? GenderId { get; set; }
    public int? AgeTo { get; set; } = null;
    public int? AgeFrom { get; set; } = null;
    public string? CaltulationType { get; set; } = null;
    public string? CaltulationSum { get; set; } = null;
    public List<string>? LanguageCommunication { get; set; } = null;

    public List<string>? Interests { get; set; } = null;

    /*public List<int> MyInterestsId { get; set; }
    public List<int> MainСategoriesId { get; set; }*/
    public double? Latitude { get; set; } = null;
    public double? Longitude { get; set; } = null;
}