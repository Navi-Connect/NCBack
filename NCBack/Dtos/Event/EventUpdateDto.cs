using System.ComponentModel.DataAnnotations.Schema;

namespace NCBack.Dtos.Event;
using Models;

public class EventUpdateDto
{
    public int AimOfTheMeetingId { get; set; }
    public int MeetingCategoryId { get; set; }
    public int MeatingPlaceId { get; set; }
    public string? IWant { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeFinish { get; set; }
    public string? City { get; set; }
    public string? Gender { get; set; }
    public int? AgeTo { get; set; }
    public int? AgeFrom { get; set; }
    public string? CaltulationType { get; set; }
    public string? CaltulationSum { get; set; }
    public List<string>? LanguageCommunication { get; set; }
    public List<string>? Interests { get; set; }
    /*public List<int> MyInterestsId { get; set; }
    public List<int> MainСategoriesId { get; set; }*/
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}