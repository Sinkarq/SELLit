using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SELLit.Server.Features.Identity.Commands.Login;
using SELLit.Server.Features.Identity.Commands.Register;
using SELLit.Server.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace SELLit.Server.Features.Identity;

[SwaggerTag("Register and login user")]
public sealed class IdentityController : ApiController
{
    [HttpPost]
    [Route(Routes.Identity.Register)]
    [SwaggerOperation("Registers a user")]
    [SwaggerResponse(204, "Registers a user")]
    [SwaggerResponse(400, "Errors occured", typeof(ErrorModel))]
    public async Task<IActionResult> Register(
        [FromBody]
        RegisterCommand model) =>
        (await this.Mediator.Send(model)).Match<IActionResult>(
            registered => this.NoContent(),
            wrongCredentials =>
                this.BadRequest(new ErrorModel(
                    wrongCredentials.Errors.Select<IdentityError, string>(x => x.Description), 400)));

    [HttpPost]
    [Route(Routes.Identity.Login)]
    [SwaggerOperation("Logins a user")]
    [SwaggerResponse(204, "Returns a login token", typeof(LoginCommandResponseModel))]
    [SwaggerResponse(400, "Invalid login credentials", typeof(ErrorModel))]
    public async Task<IActionResult> Login(
        [FromBody]
        LoginCommand requestModel)
    {
        var response = await this.Mediator.Send(requestModel);
        return response.Match<IActionResult>(
            loginCommandOutputModel => this.Ok(loginCommandOutputModel),
            wrongCredentials => this.BadRequest("Invalid login credentials."));
    }
}