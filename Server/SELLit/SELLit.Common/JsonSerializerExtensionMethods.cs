using System.Text.Json;
using CommunityToolkit.Diagnostics;

namespace SELLit.Common;

public static class JsonSerializerExtensionMethods
{
    public static async Task<T> DeserializeHttpContentAsync<T>(this Task<HttpResponseMessage> responseMessage)
    {
        var response = await responseMessage;
        Guard.IsNotNull(response, "client failed to fetch result");
        
        var content = await response.Content.ReadAsStringAsync();
        var responseModel = JsonSerializer.Deserialize<T>(content, JsonSerializerHelperClass.Options);
        Guard.IsNotNull(responseModel, "json serializer failed to serialize content");

        return responseModel;
    }
    
    public static async Task<T> DeserializeHttpContentAsync<T>(this HttpResponseMessage? responseMessage)
    {
        Guard.IsNotNull(responseMessage, "client failed to fetch result");
        var content = await responseMessage.Content.ReadAsStringAsync();
        var responseModel = JsonSerializer.Deserialize<T>(content, JsonSerializerHelperClass.Options);
        Guard.IsNotNull(responseModel, "json serializer failed to serialize content");
        
        return responseModel;
    }
}