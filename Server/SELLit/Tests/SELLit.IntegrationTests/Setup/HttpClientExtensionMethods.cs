using System.Net.Http.Json;
using System.Text.Json;

namespace SELLit.IntegrationTests.Setup;

public static class HttpClientExtensionMethods
{
    private static readonly JsonSerializerOptions options = new() {PropertyNameCaseInsensitive = true};
    
    #region GET
    public static async Task<TResponse> DeserializeGetShouldBeWithStatusCodeAsync<TResponse>(
        this Task<HttpClient> inputClient, string route, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.GetAsync(route);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content,options);

        response.StatusCode.Should().Be(statusCode);

        return result;
    }
    
    public static async Task<TResponse> DeserializeGetShouldBeWithStatusCodeAsync<TResponse>(
        this HttpClient client, string route, HttpStatusCode statusCode)
    {
        var response = await client.GetAsync(route);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content,options);

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
    #endregion

    #region POST
    public static async Task<TResponse> DeserializePostAsJsonShouldBeWithStatusCodeAsync<TResponse, TInput>(
        this Task<HttpClient> inputClient, string route, TInput input, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.PostAsJsonAsync(route, input);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content,options);

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
    #endregion

    #region PUT
    public static async Task<TResponse> DeserializePutAsJsonShouldBeWithStatusCodeAsync<TResponse, TInput>(
        this Task<HttpClient> inputClient, string route, TInput input, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.PutAsJsonAsync(route, input);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content,options);

        response.StatusCode.Should().Be(statusCode);

        return result;
    }
    
    public static async Task PutAsJsonShouldBeWithStatusCodeAsync<TInput>(
        this Task<HttpClient> inputClient, string route, TInput input, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.PutAsJsonAsync(route, input);

        response.StatusCode.Should().Be(statusCode);
    }
    #endregion

    #region DELETE

    public static async Task DeleteShouldBeWithStatusCodeAsync(
        this Task<HttpClient> inputClient, string route, HttpStatusCode statusCode)
    {
        var client = await inputClient;
        var response = await client.DeleteAsync(route);

        response.StatusCode.Should().Be(statusCode);
    }

    #endregion
}