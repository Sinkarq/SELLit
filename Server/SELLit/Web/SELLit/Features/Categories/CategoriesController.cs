using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SELLit.Common;
using SELLit.Server.Features.Categories.Commands.Create;
using SELLit.Server.Features.Categories.Commands.Delete;
using SELLit.Server.Features.Categories.Commands.Update;
using SELLit.Server.Features.Categories.Queries.Get;
using SELLit.Server.Features.Categories.Queries.GetAll;
using SELLit.Server.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace SELLit.Server.Features.Categories;

[SwaggerTag("Create, read, update and delete categories")]
public sealed class CategoriesController : ApiController
{
    private readonly IHashids hashids;

    public CategoriesController(IHashids hashids) => this.hashids = hashids;
    
    
    [HttpGet]
    [Route(Routes.Categories.GetAll)]
    [SwaggerOperation(Summary = "Gets all categories")]
    [SwaggerResponse(200, "Gets all categories", typeof(List<GetAllCategoriesQueryResponseModel>))]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
    {
        var categories = await this.Mediator.Send(GetAllCategoriesQuery.Instance, ct);

        return this.Ok(categories);
    }

    [HttpGet]
    [Route(Routes.Categories.Get)]
    [SwaggerOperation(Summary = "Returns category")]
    [SwaggerResponse(200, "Returns category", typeof(GetCategoryQueryResponseModel))]
    [SwaggerResponse(404, "Doesn't find category")]
    public async Task<IActionResult> GetCategory(
        [FromRoute] GetCategoryQuery query,
        CancellationToken ct)
    {
        var category = await this.Mediator.Send(query, ct);

        if (category is null)
        {
            return this.NotFound();
        }

        return this.Ok(category);
    }
    
    [HttpPost]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route(Routes.Categories.Create)]
    [SwaggerOperation(Summary = "Creates category")]
    [SwaggerResponse(200, "Returns created category", typeof(CreateCategoryCommandResponseModel))]
    [SwaggerResponse(400, "Error occurs", typeof(ErrorResponse))]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryCommand command,
        CancellationToken ct) =>
        (await this.Mediator.Send(command, ct)).Match<IActionResult>(
            category =>
            {
                var id = this.hashids.Encode(category.Id);
                return this.CreatedAtAction("GetCategory", new {id}, category);
            },
            y => this.BadRequest(new ErrorResponse(y.Message)));

    [HttpDelete]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route(Routes.Categories.Delete)]
    [SwaggerOperation(Summary = "Deletes category")]
    [SwaggerResponse(204, "Deletes category")]
    [SwaggerResponse(404, "Doesn't find the category to delete")]
    public async Task<IActionResult> DeleteCategory(
        [FromRoute] DeleteCategoryCommand command,
        CancellationToken ct) =>
        (await this.Mediator.Send(command, ct)).Match<IActionResult>(
            deleted => this.NoContent(),
            notFound => this.NotFound()
        );

    [HttpPut]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route(Routes.Categories.Update)]
    [SwaggerOperation(Summary = "Updates category")]
    [SwaggerResponse(200, "Updates category", typeof(UpdateCategoryCommandResponseModel))]
    [SwaggerResponse(404, "Doesn't find the category to update")]
    [SwaggerResponse(400, "Error occurs", typeof(ErrorResponse))]
    public async Task<IActionResult> UpdateCategory(
        [FromBody] UpdateCategoryCommand command,
        CancellationToken ct)
        => (await this.Mediator.Send(command, ct)).Match<IActionResult>(
            updated => this.Ok(),
            notFound => this.NotFound(),
            error => this.BadRequest(new ErrorResponse(error.Message)) 
        );
}