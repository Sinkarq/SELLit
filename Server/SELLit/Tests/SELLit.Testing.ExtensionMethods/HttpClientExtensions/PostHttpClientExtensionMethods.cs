namespace SELLit.Testing.ExtensionMethods.HttpClientExtensions;

public static partial class HttpClientExtensionMethods
{
    public static async Task<TResponse> DeserializePostAsJsonShouldBeWithStatusCodeAsync<TResponse, TInput>(
        this Task<HttpClient> inputClient, string route, TInput input, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.PostAsJsonAsync(route, input);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content,JsonSerializerHelperClass.Options);

        response.StatusCode.Should().Be(statusCode);

        return result;
    }
    
    public static async Task<HttpResponseMessage> PostAsJsonShouldBeWithStatusCodeAsync<TInput>(
        this Task<HttpClient> inputClient, string route, TInput input, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.PostAsJsonAsync(route, input);

        response.StatusCode.Should().Be(statusCode);

        return response;
    }
}