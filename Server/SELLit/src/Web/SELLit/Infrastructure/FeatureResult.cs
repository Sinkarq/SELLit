using Microsoft.AspNetCore.Mvc;
using SELLit.Common;

namespace SELLit.Server.Infrastructure;

public readonly struct FeatureResult<TValue>
{
    public ActionResult<TValue> Result { get; }
    
    public FeatureResult(ActionResult<TValue> result)
    {
        NullGuardMethods.Guard(result);
        this.Result = result;
    }
}