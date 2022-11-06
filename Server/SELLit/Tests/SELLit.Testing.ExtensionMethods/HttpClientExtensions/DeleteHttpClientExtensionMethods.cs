namespace SELLit.Testing.ExtensionMethods.HttpClientExtensions;

public static partial class HttpClientExtensionMethods
{
    public static async Task DeleteShouldBeWithStatusCodeAsync(
        this Task<HttpClient> inputClient, string route, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.DeleteAsync(route);

        response.StatusCode.Should().Be(statusCode);
    }
}