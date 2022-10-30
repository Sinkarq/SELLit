using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using SELLit.Common;
using SELLit.Server.Features;
using SELLit.Server.Features.Categories.Queries.Get;
using Xunit;

namespace SELLit.Tests.IntegrationTests.CategoriesController;

public class GetAllCategoriesTests : IntegrationTest
{
    [Fact]
    public void Should_Return_Two_Categories()
    {
        var response = this.HttpClient.GetAsync(Routes.Categories.GetAll).GetAwaiter().GetResult();
        var responseModel = response.DeserializeHttpContentAsync<List<GetCategoryQueryResponseModel>>().GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseModel.Should().HaveCountGreaterThan(1);
        responseModel.Should().NotContainNulls();
    }
}