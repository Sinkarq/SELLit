using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SELLit.Data;
using SELLit.Data.Models;
using SELLit.Data.Repositories;
using SELLit.Server.Features;
using Xunit;

namespace SELLit.Tests.IntegrationTests.CategoriesController;

public class DeleteCategoryTests : IntegrationTest
{
    [Fact]
    public async Task DeleteCategory_SafeDelete()
    {
        await this.AuthenticateAdminAsync();
        
        var dbContext = this.ServiceProvider.GetService<ApplicationDbContext>();
        var repository = new EfDeletableEntityRepository<Category>(dbContext);
        
        var hashids = this.ServiceProvider.GetService<IHashids>();

        var url = Routes.Categories.Delete.Replace("{id:hashids}", hashids.Encode(1));
        ;
        var beforeCount = await repository.AllAsNoTracking().CountAsync();
        var response = await this.HttpClient.DeleteAsync(url);
        var afterCount = await repository.AllAsNoTracking().CountAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        beforeCount.Should().NotBe(afterCount);
    }
    
    [Fact]
    public async Task DeleteCategory_NotFound()
    {
        await this.AuthenticateAdminAsync();

        var hashids = this.ServiceProvider.GetService<IHashids>();

        var url = Routes.Categories.Delete.Replace("{id:hashids}", hashids.Encode(69));

        var response = await this.HttpClient.DeleteAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    [Fact]
    public async Task DeleteCategory_NotAuthorized()
    {
        await this.AuthenticateAsync();

        var hashids = this.ServiceProvider.GetService<IHashids>();

        var url = Routes.Categories.Delete.Replace("{id:hashids}", hashids.Encode(1));

        var response = await this.HttpClient.DeleteAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}