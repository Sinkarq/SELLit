namespace SELLit.Server.Infrastructure;

public class ErrorResponseModel
{
    public ErrorResponseModel()
    {
    }

    public ErrorResponseModel(string message)
    {
        Message = message;
    }

    public string FieldName { get; set; } = default!;
    public string Message { get; set; } = "Unknown";
}