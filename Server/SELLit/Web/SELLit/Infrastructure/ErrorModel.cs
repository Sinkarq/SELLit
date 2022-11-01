using System.ComponentModel;
using System.Text.Json.Serialization;
using SELLit.Common;

namespace SELLit.Server.Infrastructure;

public readonly struct ErrorModel
{
    [JsonConstructor]
    public ErrorModel(IEnumerable<string> errors, int statusCode)
    {
        /*NullGuardMethods.Guard(statusCode);
        NullGuardMethods.Guard(errors);*/
        Errors = errors;
        StatusCode = statusCode;
    }

    [DisplayName("errors")]
    public IEnumerable<string> Errors { get; }
    
    [DisplayName("status")]
    public int StatusCode { get; }
}