using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NCBack.Models;

public class Event
{
    public int Id { get; set; }
    public string? AimOfTheMeeting { get; set; }
    public string? MeetingCategory { get; set; }
    public string? MeatingName { get; set; }
    public DateTime? Date { get; set; }
    public TimeSpan TimeStart { get; set; }
    public TimeSpan TimeFinish { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Gender { get; set; }
    public int? AgeTo { get; set; }
    public int? AgeFrom { get; set; }
    public string? CaltulationType { get; set; }
    public string? CaltulationSum { get; set; }
    public string? LanguageCommunication { get; set; }
    public string? MeatingPlace { get; set; }
    public string? MeatingInterests{ get; set; }
    public int? UsreId { get; set; }
    [NotMapped]
    public User? User { get; set; }
    
}