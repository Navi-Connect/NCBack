namespace NCBack.Models;

public class MeatingPlace
{
    public int Id { get; set; }
    public string NameMeatingPlace { get; set; }
    public int MeetingCategoryId { get; set; }
    public MeetingCategory MeetingCategory { get; set; }
}