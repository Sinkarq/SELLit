namespace SELLit.Server.Infrastructure;

public class ErrorResponse
{
    public List<ErrorResponseModel> Errors { get; set; } = new();

    public ErrorResponse()
    {
    }

    public ErrorResponse(string message) => this.Errors.Add(new ErrorResponseModel() {Message = message});
}