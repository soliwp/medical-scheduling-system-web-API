using Microsoft.EntityFrameworkCore;
using MS.Domain.Entities.authentication;

namespace MS.Infrastructure.EF.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MSContext _context;

    public UserRepository(MSContext context)
    {
        _context = context;
    }

    public async Task<User> Login(User user)
    {        
        return await _context.users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
    }
}