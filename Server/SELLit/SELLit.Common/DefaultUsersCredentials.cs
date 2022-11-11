namespace SELLit.Common;

public  class DefaultUsersCredentials
{
    public static UserCredentials DefaultUser = new UserCredentials("John", "password1234");
    public static UserCredentials AdminUser = new UserCredentials("Sinkarq", "password1234");
}

public class UserCredentials
{
    public UserCredentials(string username, string password)
    {
        this.Username = username;
        this.Password = password;
    }
    
    public string Username { get; init; }
    public string Password { get; init; }
}