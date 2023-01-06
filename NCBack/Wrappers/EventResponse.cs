namespace NCBack.Wrappers
{
    public class EventResponse<Event>
    {
        public EventResponse()
        {
        }
        public EventResponse(Event data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public Event Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}