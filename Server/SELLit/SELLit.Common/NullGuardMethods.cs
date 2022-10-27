namespace SELLit.Common;


/// <summary>
/// Guards string value against nulls.
/// </summary>
public static class NullGuardMethods
{
    public static void Guard<T>(T value)
    {
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value);
    }

    public static void Guard<T>(params T[] objects)
    {
        foreach (var obj in objects)
        {
            Guard(obj);
        }
    } 
}