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

    public static void Guard<T1, T2>(T1 value1, T2 value2)
    {
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value1);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value2);
    }
    public static void Guard<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value1);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value2);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value3);
    }
    
    public static void Guard<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
    {
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value1);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value2);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value3);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value4);
    }
    
    public static void Guard<T1, T2, T3, T4, T5>(
        T1 value1, 
        T2 value2, 
        T3 value3, 
        T4 value4, 
        T5 value5)
    {
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value1);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value2);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value3);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value4);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value5);
    }
    
    public static void Guard<T1, T2, T3, T4, T5, T6>(
        T1 value1, 
        T2 value2, 
        T3 value3, 
        T4 value4, 
        T5 value5,
        T6 value6)
    {
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value1);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value2);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value3);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value4);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value5);
        CommunityToolkit.Diagnostics.Guard.IsNotNull(value6);
    }
}