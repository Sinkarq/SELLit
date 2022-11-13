namespace SELLit.Common;

public  class DefaultUsersCredentials
{
    public static UserCredentials DefaultUser = new()
    {
        Username = "John",
        Password = "password1234"
    };
    public static UserCredentials AdminUser = new()
    {
        Username = "Sinkarq",
        Password = "password1234"
    };
}

public class UserCredentials
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}