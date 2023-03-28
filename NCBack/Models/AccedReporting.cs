namespace NCBack.Models;


public class AccedReporting
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public DateTime TimeCreat { get; set; }
    public string? ReportingsNotification { get; set; }
}