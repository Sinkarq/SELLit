using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SELLit.Server.Features.Categories.Queries.GetAll;
using SELLit.Server.Features.Products.Commands.Create;
using SELLit.Server.Features.Products.Commands.Delete;
using SELLit.Server.Features.Products.Commands.Update;
using SELLit.Server.Features.Products.Queries.Get;
using SELLit.Server.Features.Products.Queries.GetAll;
using SELLit.Server.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace SELLit.Server.Features.Products;

[Authorize]
[SwaggerTag("Create, read, update and delete products")]
public sealed class ProductsController : ApiController
{
    private readonly IHashids hashids;

    public ProductsController(IHashids hashids) => this.hashids = hashids;

    [HttpGet]
    [AllowAnonymous]
    [Route(Routes.Products.GetAll)]
    [SwaggerOperation(Summary = "Gets all products")]
    [SwaggerResponse(200, "Gets all products", typeof(List<GetAllCategoriesQueryResponseModel>))]
    public async Task<IActionResult> GetProducts()
        => this.Ok(await this.Mediator.Send(new GetAllProductsQuery()));

    [HttpGet]
    [AllowAnonymous]
    [Route(Routes.Products.Get)]
    [SwaggerOperation(Summary = "Returns category")]
    [SwaggerResponse(200, "Returns category", typeof(GetProductQueryResponseModel))]
    [SwaggerResponse(404, "Doesn't find category")]
    public async Task<IActionResult> GetProduct(
        [FromRoute] GetProductQuery query)
    {
        var product = await this.Mediator.Send(query);

        if (product is null)
        {
            return this.NotFound();
        }

        return this.Ok(product);
    }

    [HttpPost]
    [Route(Routes.Products.Create)]
    [SwaggerOperation(Summary = "Creates product")]
    [SwaggerResponse(200, "Returns created product", typeof(CreateProductCommandResponseModel))]
    [SwaggerResponse(400, "Error occurs", typeof(ErrorResponse))]
    public async Task<IActionResult> CreateProduct(
        [FromBody]
        CreateProductCommand command)
    {
        var product = await this.Mediator.Send(command);
        var id = this.hashids.Encode(product.Id);

        if (product is null)
        {
            return this.BadRequest();
        }

        return this.CreatedAtAction("GetProduct", new {id}, product);
    }

    [HttpPut]
    [Route(Routes.Products.Update)]
    [SwaggerOperation(Summary = "Updates product")]
    [SwaggerResponse(200, "Returns updated category", typeof(UpdateProductCommandResponseModel))]
    [SwaggerResponse(404, "Doesn't find the category to update")]
    [SwaggerResponse(403, "Doesn't have permissions to update this product, only the creator of it has")]
    public async Task<IActionResult> UpdateProduct(
        [FromBody] UpdateProductCommand command)
    {
        var response = await this.Mediator.Send(command);

        return response.Match<IActionResult>(
            updatedProduct => this.Ok(updatedProduct),
            notFound => this.NotFound(),
            forbidden => this.Forbid());
    }

    [HttpDelete]
    [Route(Routes.Products.Delete)]
    [SwaggerOperation(Summary = "Deletes product")]
    [SwaggerResponse(204, "Deletes product")]
    [SwaggerResponse(404, "Doesn't find the product to delete")]
    [SwaggerResponse(403, "Doesn't have permissions to update this product, only the creator of it has")]
    public async Task<IActionResult> DeleteProduct(
        [FromRoute] DeleteProductCommand command)
        => (await this.Mediator.Send(command)).Match<IActionResult>(
            deleted => this.Ok(),
            notFound => this.NotFound(),
            forbidden => this.Forbid());
}