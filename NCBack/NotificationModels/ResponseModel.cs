using Newtonsoft.Json;

namespace NCBack.NotificationModels;

public class ResponseModel
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; }
        
    [JsonProperty("message")]
    public string Message { get; set; }
}