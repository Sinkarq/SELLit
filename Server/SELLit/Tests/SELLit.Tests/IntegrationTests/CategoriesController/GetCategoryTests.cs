using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using HashidsNet;
using Microsoft.Extensions.DependencyInjection;
using SELLit.Common;
using SELLit.Server.Features;
using SELLit.Server.Features.Categories.Queries.Get;
using Xunit;

namespace SELLit.Tests.IntegrationTests.CategoriesController;

public class GetCategoryTests : IntegrationTest
{
    [Fact]
    public void GetCategory_Should_Return_Valid()
    {
        var hashids = this.ServiceProvider.GetService<IHashids>();

        var url = Routes.Categories.Get.Replace("{id:hashids}", hashids.Encode(1));
        
        var response = this.HttpClient.GetAsync(url).GetAwaiter().GetResult();
        var responseModel = response.DeserializeHttpContentAsync<GetCategoryQueryResponseModel>().GetAwaiter().GetResult();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseModel.Should().NotBeNull();
    }
    
    [Fact]
    public void GetCategory_Should_Return_NotFound()
    {
        var hashids = this.ServiceProvider.GetService<IHashids>();

        var url = Routes.Categories.Get.Replace("{id:hashids}", hashids.Encode(69));
        
        var response = this.HttpClient.GetAsync(url).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}