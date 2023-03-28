namespace NCBack.Models;

public enum AccedNotification {
    NotVerified,
    Verified,
}
public class AccedEventUser
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public  User User { get; set; }
    public int EventId { get; set; }
    public  Event Event { get; set; }
    public DateTime TimeStartEventUser { get; set; } 
    public DateTime TimeFinishEventUser { get; set; }
    
    public AccedNotification AccedNotifications { get; set; } = AccedNotification.NotVerified;
    public AccedEventUser(int userId, int eventId, DateTime timeStartEventUser, DateTime timeFinishEventUser)
    {
        UserId = userId;
        EventId = eventId;
        TimeStartEventUser = timeStartEventUser;
        TimeFinishEventUser = timeFinishEventUser;
    }
}