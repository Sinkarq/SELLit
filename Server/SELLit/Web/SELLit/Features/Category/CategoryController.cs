using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SELLit.Common;
using SELLit.Server.Features.Category.Commands.Create;
using SELLit.Server.Features.Category.Commands.Delete;
using SELLit.Server.Features.Category.Commands.Rename;
using SELLit.Server.Features.Category.Queries.Get;
using SELLit.Server.Features.Category.Queries.GetAll;

namespace SELLit.Server.Features.Category;

public class CategoryController : ApiController
{
    private readonly IHashids hashids;

    public CategoryController(IHashids hashids) => this.hashids = hashids;

    [HttpPost]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
    {
        var response = await this.Mediator.Send(command);

        var id = this.hashids.Encode(response.Id);

        return this.CreatedAtAction("GetCategory", new {id}, new { });
    }

    [HttpDelete]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> DeleteCategory(
        [FromBody] DeleteCategoryCommand command) =>
        (await this.Mediator.Send(command)).Match<IActionResult>(
            _ => this.Ok(),
            _ => this.NotFound()
        );

    [HttpPut]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> RenameCategory(
        [FromBody] RenameCategoryCommand command)
        => (await this.Mediator.Send(command)).Match<IActionResult>(
            _ => this.Ok(),
            _ => this.NotFound()
        );

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetAllCategoriesQuery();
        var categories = await this.Mediator.Send(query);

        return this.Ok(categories);
    }

    [HttpGet]
    [Route("/[controller]/{id:hashids}")]
    public async Task<IActionResult> GetCategory([FromRoute] GetCategoryQuery query)
        => (await this.Mediator.Send(query)).Match<IActionResult>(
            category => this.Ok(category),
            _ => this.NotFound());
}