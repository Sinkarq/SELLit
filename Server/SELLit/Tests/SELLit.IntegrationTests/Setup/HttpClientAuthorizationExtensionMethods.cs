using System.Net.Http.Headers;
using System.Net.Http.Json;
using SELLit.Common;
using SELLit.Server.Features.Identity.Commands.Login;

namespace SELLit.IntegrationTests.Setup;

internal static class HttpClientAuthorizationExtensionMethods
{
    internal static async Task<HttpClient> WithAdminAuthentication(this HttpClient client)
    {
        var response = await client.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "Sinkarq",
            Password = "password1234"
        }).DeserializeHttpContentAsync<LoginCommandResponseModel>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", response.Token);

        return client;
    }
    
    internal static async Task<HttpClient> WithDefaultAuthentication(this HttpClient client)
    {
        var response = await client.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = "John",
            Password = "password1234"
        }).DeserializeHttpContentAsync<LoginCommandResponseModel>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", response.Token);

        return client;
    }
    
    internal static async Task<HttpClient> WithNoAuthentication(this HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", string.Empty);

        return client;
    }
}