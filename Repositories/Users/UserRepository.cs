using Microsoft.EntityFrameworkCore;
using MovieAPI_dotnet.Data;
using MovieAPI_dotnet.Models;

namespace MovieAPI_dotnet.Repositories.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string Email);
        Task AddUser(User user);
    }
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _context.Users
                .Where(r => r.Email.Contains(email))
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task AddUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
