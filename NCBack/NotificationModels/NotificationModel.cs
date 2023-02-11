using Newtonsoft.Json;

namespace NCBack.NotificationModels;

public class NotificationModel
{
    [JsonProperty("Id")]
    public int Id { get; set; }  
    [JsonProperty("UserId")]
    public int UserId { get; set; }

    [JsonProperty("deviceId")]
    public string? DeviceId { get; set; } = null;

    [JsonProperty("isAndroiodDevice")]
    public bool IsAndroiodDevice { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }
    [JsonProperty("date")]
    public DateTime DateTime { get; set; } = DateTime.Now;

    public Boolean Status { get; set; } = false;

    public NotificationModel(int id, int userId, bool isAndroiodDevice, string title, string body, DateTime dateTime, Boolean status)
    {
        Id = id;
        UserId = userId;
        IsAndroiodDevice = isAndroiodDevice;
        Title = title;
        Body = body;
        DateTime = dateTime;
        Status = status;
    } 
    public NotificationModel()
    {
    }
}

public class GoogleNotification
{
    public class DataPayload
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    [JsonProperty("priority")]
    public string Priority { get; set; } = "high";

    [JsonProperty("data")]
    public DataPayload Data { get; set; }

    [JsonProperty("notification")]
    public DataPayload Notification { get; set; }
}