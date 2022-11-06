using System.Text.Json;

namespace SELLit.Common;

public static class JsonSerializerHelperClass
{
    public static readonly JsonSerializerOptions Options = new() {PropertyNameCaseInsensitive = true};
}