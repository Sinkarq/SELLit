using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SELLit.Server.Features;

[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
    private IMediator? mediator;

    protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
}
