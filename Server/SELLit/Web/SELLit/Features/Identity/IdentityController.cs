using Microsoft.AspNetCore.Mvc;
using SELLit.Server.Features.Identity.Commands.Login;
using SELLit.Server.Features.Identity.Commands.Register;
using SELLit.Server.Infrastructure;

namespace SELLit.Server.Features.Identity;

public sealed class IdentityController : ApiController
{
    [HttpPost]
    [Route(nameof(Register))]
    public async Task<IActionResult> Register(RegisterCommandRequestModel model) =>
        (await this.Mediator.Send(model)).Match<IActionResult>(
            _ => this.Ok(),
            credentials => this.BadRequest(new ErrorModel(credentials.Errors.Select(x => x.Description), 400)));

    [HttpPost]
    [Route(nameof(Login))]
    public async Task<IActionResult> Login(LoginCommandRequestModel model)
    {
        var outputModel = await this.Mediator.Send(model);
        return outputModel.Match<IActionResult>(
            loginCommandOutputModel => this.Ok(loginCommandOutputModel),
            _ => this.BadRequest("Invalid login credentials"));
    }
}