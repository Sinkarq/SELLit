using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SELLit.Server.Features;

[ApiController]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
    private IMediator? mediator;

    protected IMediator Mediator
    {
        get 
        {
            if (mediator is null)
            {
                this.mediator = this.HttpContext.RequestServices.GetService<IMediator>();
            }
            
            Guard.IsNotNull(this.mediator, "IMediator is not provided in the service collection");

            return mediator;
        }
    }
}
