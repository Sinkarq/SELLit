using System.Net.Http.Headers;
using SELLit.Server.Features;
using SELLit.Server.Features.Identity.Commands.Login;

namespace SELLit.Testing.ExtensionMethods.HttpClientExtensions;

public static partial class HttpClientExtensionMethods
{
    public static async Task<HttpClient> WithAdminAuthentication(this HttpClient client)
    {
        var response = await client.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = DefaultUsersCredentials.AdminUser.Username,
            Password = DefaultUsersCredentials.AdminUser.Password
        }).DeserializeHttpContentAsync<LoginCommandResponseModel>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", response.Token);

        return client;
    }
    
    public static async Task<HttpClient> WithDefaultAuthentication(this HttpClient client)
    {
        var response = await client.PostAsJsonAsync(Routes.Identity.Login, new LoginCommand()
        {
            Username = DefaultUsersCredentials.DefaultUser.Username,
            Password = DefaultUsersCredentials.DefaultUser.Password
        }).DeserializeHttpContentAsync<LoginCommandResponseModel>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", response.Token);

        return client;
    }
    
    public static Task<HttpClient> WithNoAuthentication(this HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", string.Empty);

        return Task.FromResult(client);
    }
}