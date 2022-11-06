namespace SELLit.Testing.ExtensionMethods.HttpClientExtensions;

public static partial class HttpClientExtensionMethods
{
    public static async Task<TResponse> DeserializeGetShouldBeWithStatusCodeAsync<TResponse>(
        this Task<HttpClient> inputClient, string route, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.GetAsync(route);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content,JsonSerializerHelperClass.Options);

        response.StatusCode.Should().Be(statusCode);

        return result;
    }
    
    public static async Task<TResponse> DeserializeGetShouldBeWithStatusCodeAsync<TResponse>(
        this HttpClient client, string route, HttpStatusCode statusCode)
    {
        var response = await client.GetAsync(route);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content,JsonSerializerHelperClass.Options);

        response.StatusCode.Should().Be(statusCode);

        return result;
    }
    
    public static async Task GetShouldBeWithStatusCodeAsync(
        this Task<HttpClient> inputClient, string route, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.GetAsync(route);

        response.StatusCode.Should().Be(statusCode);
    }
    
    public static async Task GetShouldBeWithStatusCodeAsync(
        this HttpClient client, string route, HttpStatusCode statusCode)
    {
        var response = await client.GetAsync(route);

        response.StatusCode.Should().Be(statusCode);
    }
}