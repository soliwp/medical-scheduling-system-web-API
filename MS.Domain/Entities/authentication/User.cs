namespace MS.Domain.Entities.authentication;
public class User
{

    public int UserId { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public Role Role { get; private set; }
    public User(string email, string password)
    {        
        Email = email;
        Password = password;        
    }
    public User(int id , string email, string password)
    {        
        UserId = id;
        Email = email;
        Password = password;        
    }
}
