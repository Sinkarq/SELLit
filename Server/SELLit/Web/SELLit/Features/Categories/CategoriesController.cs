using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SELLit.Common;
using SELLit.Server.Features.Categories.Commands.Create;
using SELLit.Server.Features.Categories.Commands.Delete;
using SELLit.Server.Features.Categories.Commands.Update;
using SELLit.Server.Features.Categories.Queries.Get;
using SELLit.Server.Features.Categories.Queries.GetAll;

namespace SELLit.Server.Features.Categories;

public class CategoriesController : ApiController
{
    private readonly IHashids hashids;

    public CategoriesController(IHashids hashids) => this.hashids = hashids;
    
    [HttpGet]
    [Route(Routes.Categories.GetAll)]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetAllCategoriesQuery();
        var categories = await this.Mediator.Send(query);

        return this.Ok(categories);
    }

    [HttpGet]
    [Route(Routes.Categories.Get)]
    public async Task<IActionResult> GetCategory(
        [FromRoute] 
        GetCategoryQuery query)
        => (await this.Mediator.Send(query)).Match<IActionResult>(
            category => this.Ok(category),
            _ => this.NotFound());

    [HttpPost]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route(Routes.Categories.Create)]
    public async Task<IActionResult> CreateCategory(
        [FromBody]
        CreateCategoryCommand command)
    {
        var response = await this.Mediator.Send(command);

        var id = this.hashids.Encode(response.Id);

        return this.CreatedAtAction("GetCategory", new {id}, new { });
    }

    [HttpDelete]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route(Routes.Categories.Delete)]
    public async Task<IActionResult> DeleteCategory(
        [FromRoute] DeleteCategoryCommand command) =>
        (await this.Mediator.Send(command)).Match<IActionResult>(
            _ => this.Ok(),
            _ => this.NotFound()
        );

    [HttpPut]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route(Routes.Categories.Update)]
    public async Task<IActionResult> RenameCategory(
        [FromBody] UpdateCategoryCommand command)
        => (await this.Mediator.Send(command)).Match<IActionResult>(
            _ => this.Ok(),
            _ => this.NotFound()
        );
}