using Microsoft.AspNetCore.Mvc;
using SELLit.Server.Features.Identity.Commands.Login;
using SELLit.Server.Features.Identity.Commands.Register;
using SELLit.Server.Infrastructure;

namespace SELLit.Server.Features.Identity;

public sealed class IdentityController : ApiController
{
    [HttpPost]
    [Route(Routes.Identity.Register)]
    public async Task<IActionResult> Register(
        [FromBody]
        RegisterCommand model) =>
        (await this.Mediator.Send(model)).Match<IActionResult>(
            _ => this.Ok(),
            credentials => this.BadRequest(new ErrorModel(credentials.Errors.Select(x => x.Description), 400)));

    [HttpPost]
    [Route(Routes.Identity.Login)]
    public async Task<IActionResult> Login(
        [FromBody]
        LoginCommand requestModel)
    {
        var response = await this.Mediator.Send(requestModel);
        return response.Match<IActionResult>(
            loginCommandOutputModel => this.Ok(loginCommandOutputModel),
            _ => this.BadRequest("Invalid login credentials."));
    }
}