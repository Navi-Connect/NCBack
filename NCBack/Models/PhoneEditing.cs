namespace NCBack.Models;

public class PhoneEditing
{
    public int? Id { get; set; } 
    public string? PhoneNumber  { get; set; }
    public int? Code { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}