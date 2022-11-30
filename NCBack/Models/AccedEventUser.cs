namespace NCBack.Models;

public class AccedEventUser
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public  User User { get; set; }
    public int EventId { get; set; }
    public  Event Event { get; set; }
    
    public AccedEventUser(int userId, int eventId)
    {
        UserId = userId;
        EventId = eventId;
    }
}