using StatSanctum.Entities;
using StatSanctum.Repositories;

namespace StatSanctum.API.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByUsernameEmailAsync(string usernameEmail);
        Task<(int, string?)> ValidateUserCredentials(string usernameEmail, string password);
    }
}
