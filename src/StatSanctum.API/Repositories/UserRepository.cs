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
    }
}
