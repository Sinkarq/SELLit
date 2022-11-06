using System.Text.Json;

namespace SELLit.Common;

public static class JsonSerializerExtensionMethods
{
    public static async Task<T> DeserializeHttpContentAsync<T>(this Task<HttpResponseMessage> responseMessage)
    {
        var response = await responseMessage;
        var content = await response.Content.ReadAsStringAsync();
        var responseModel = JsonSerializer.Deserialize<T>(content, JsonSerializerHelperClass.Options);

        return responseModel;
    }
    
    public static async Task<T> DeserializeHttpContentAsync<T>(this HttpResponseMessage responseMessage)
    {
        var content = await responseMessage.Content.ReadAsStringAsync();
        var responseModel = JsonSerializer.Deserialize<T>(content, JsonSerializerHelperClass.Options);
        return responseModel;
    }
}