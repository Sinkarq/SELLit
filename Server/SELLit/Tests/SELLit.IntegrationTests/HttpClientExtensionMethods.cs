using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace SELLit.IntegrationTests;

public static class HttpClientExtensionMethods
{
    private static readonly JsonSerializerOptions options = new() {PropertyNameCaseInsensitive = true};

    public static async Task<TResponse> PostJsonAsync<TResponse, TInput>(this HttpClient client, string route, TInput input)
    {
        var response = await client.PostAsJsonAsync(route, input);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content, options);

        return result;
    }
    
    public static async Task<TResponse> PostJsonAsyncShouldBeWithStatusCode<TResponse, TInput>(
        this HttpClient client, string route, TInput input, HttpStatusCode statusCode)
    {
        var response = await client.PostAsJsonAsync(route, input);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(content,options);

        response.StatusCode.Should().Be(statusCode);

        return result;
    }
}