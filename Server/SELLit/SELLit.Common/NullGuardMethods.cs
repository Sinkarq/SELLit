namespace SELLit.Common;


/// <summary>
/// Guards string value against nulls.
/// </summary>
public static class GuardWith
{
    public static T NotNull<T>(T value)
    {
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value);

        return value;
    }

    public static void NotNull<T>(params T[] objects)
    {
        foreach (var obj in objects)
        {
            NotNull(obj);
        }
    } 
}