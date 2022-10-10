namespace NCBack.Models;

public class UserEvent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public  User User { get; set; }
    public int EventId { get; set; }
    public  Event Event { get; set; }

    public UserEvent(int userId, int eventId)
    {
        UserId = userId;
        EventId = eventId;
    }
}