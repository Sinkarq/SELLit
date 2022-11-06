using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SELLit.Server.Features;

[ApiController]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
    private IMediator? mediator;

    protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
}
