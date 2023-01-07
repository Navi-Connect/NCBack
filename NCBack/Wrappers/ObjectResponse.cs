namespace NCBack.Wrappers;

public class ObjectResponse<T>
{
    public ObjectResponse()
    {
    }

    public ObjectResponse(object data)
    {
        Succeeded = true;
        Message = string.Empty;
        Errors = null;
        Data = data;
    }

    public object Data { get; set; }
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; }
    public string Message { get; set; }
}