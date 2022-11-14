using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.Event;
using Models;
public class EventCreateDto
{
    public string? AimOfTheMeeting { get; set; }
    public string? MeetingCategory { get; set; }
    public string? IWant { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeFinish { get; set; }
    public string? City { get; set; }
    public string? Gender { get; set; }
    public int? AgeTo { get; set; }
    public int? AgeFrom { get; set; }
    public string? CaltulationType { get; set; }
    public string? CaltulationSum { get; set; }
    public string? LanguageCommunication { get; set; }
    public string? MeatingPlace { get; set; }
    public string? MeatingInterests{ get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}