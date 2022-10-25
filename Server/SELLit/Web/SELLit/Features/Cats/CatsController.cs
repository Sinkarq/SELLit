using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SELLit.Server.Features.Cats.Commands.CreateCommand;
using SELLit.Server.Features.Cats.Queries.GetById;

namespace SELLit.Server.Features.Cats;

public sealed class CatsController : ApiController
{
    [HttpGet]
    [Route("/[controller]/{id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdQueryModel model) 
        => (await this.Mediator.Send(model))
            .Match<IActionResult>(result => this.Ok(result), _ => this.NotFound());

    // OrNotFound is a global action located in the Infrastructure Project
    // And all it does is this check
    // if (entity == null) return new NotFoundResult();
    // if not return the model
    // So it saves you everytime you have to check for
    // if you have null or valid entity that has been retrieved from the db

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCat(CreateCommandModel model)
    {
        var outputModel = await this.Mediator.Send(model);
        return this.CreatedAtAction("GetById", new {id = outputModel.Id}, outputModel);
    }
}