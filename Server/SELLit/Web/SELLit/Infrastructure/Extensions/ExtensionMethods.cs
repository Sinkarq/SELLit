namespace SELLit.Server.Infrastructure.Extensions;

public static class ExtensionMethods
{
    public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
    {
        var enumerable = array as T[] ?? array.ToArray();
        for (var i = 0; i < enumerable.Length; i++)
        {
            action(enumerable[i]);
        }
    }
}