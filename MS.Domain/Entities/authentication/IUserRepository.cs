namespace MS.Domain.Entities.authentication;
public interface IUserRepository
{
    Task<User> Login(User user);
}
