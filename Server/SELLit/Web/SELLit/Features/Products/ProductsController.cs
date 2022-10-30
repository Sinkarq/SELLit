using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SELLit.Server.Features.Products.Commands.Create;
using SELLit.Server.Features.Products.Commands.Delete;
using SELLit.Server.Features.Products.Commands.Update;
using SELLit.Server.Features.Products.Queries.Get;
using SELLit.Server.Features.Products.Queries.GetAll;

namespace SELLit.Server.Features.Products;

[Authorize]
public class ProductsController : ApiController
{
    private readonly IHashids hashids;

    public ProductsController(IHashids hashids) => this.hashids = hashids;
    
    [HttpGet]
    [AllowAnonymous]
    [Route("/[controller]/{id:hashids}")]
    public async Task<IActionResult> GetProduct(
        [FromRoute]
        GetProductQuery query) 
        => (await this.Mediator.Send(query)).Match<IActionResult>(
            _ => this.Ok(_),
            _ => this.NotFound());

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts()
        => this.Ok(await this.Mediator.Send(new GetAllProductsQuery()));

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody]
        CreateProductCommand command)
    {
        var response = await this.Mediator.Send(command);

        var id = this.hashids.Encode(response.Id);
        
        //TODO: Redirect location
        return this.CreatedAtAction("GetProduct", new {id}, new {});
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(
        [FromBody]
        UpdateProductCommand command)
    {
        var response = await this.Mediator.Send(command);

        return response.Match<IActionResult>(
            _ => this.Ok(), 
            _=> this.NotFound(), 
            _ => this.Unauthorized());
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(
        [FromBody] DeleteProductCommand command)
        => (await this.Mediator.Send(command)).Match<IActionResult>(
            _ => this.Ok(), 
            _ => this.NotFound(),
            _ => this.Unauthorized());
}