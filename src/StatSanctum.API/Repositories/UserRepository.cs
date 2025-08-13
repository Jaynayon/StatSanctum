using Microsoft.EntityFrameworkCore;
using StatSanctum.API.Helper;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Repositories;

namespace StatSanctum.API.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Username, Password, and Email must be provided.");
            }

            user.Salt = PasswordHelper.GenerateSalt();
            user.Password = PasswordHelper.HashPassword(user.Password, user.Salt);

            _dbSet.Add(user);

            return await _context.SaveChangesAsync().ContinueWith(t => user);
        }

        public async Task<User> GetUserByUsernameEmailAsync(string usernameEmail)
        {
            if(string.IsNullOrWhiteSpace(usernameEmail))
                throw new ArgumentNullException(nameof(usernameEmail));

            var user = await _dbSet
                .Where(u => u.Username == usernameEmail || u.Email == usernameEmail)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new ArgumentException("User not found");

            return user;
        }

        public async Task<(int, string?)> ValidateUserCredentials(string usernameEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(usernameEmail) ||
                string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and Password must be provided.");

            var user = await GetUserByUsernameEmailAsync(usernameEmail);

            if (user == null)
                return (0, string.Empty);

            if (string.IsNullOrWhiteSpace(user.Salt))
                throw new ArgumentException("User does not have a salt value to hash the password.");

            var hashedPassword = PasswordHelper.HashPassword(password, user.Salt);

            if(user.Password == hashedPassword)
                return (user.UserID, user.Username);

            return (0, string.Empty);
        }
    }
}
