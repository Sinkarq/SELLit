using System.ComponentModel;
using SELLit.Common;

namespace SELLit.Server.Infrastructure;

public readonly struct ErrorModel
{
    public ErrorModel(IEnumerable<string> errors, int statusCode)
    {
        NullGuardMethods.Guard(statusCode, errors);
        Errors = errors;
        StatusCode = statusCode;
    }
    
    public ErrorModel(int statusCode)
    {
        NullGuardMethods.Guard(statusCode);
        Errors = Array.Empty<string>();
        StatusCode = statusCode;
    }

    [DisplayName("errors")]
    public IEnumerable<string> Errors { get; }
    
    [DisplayName("status")]
    public int StatusCode { get; }
}