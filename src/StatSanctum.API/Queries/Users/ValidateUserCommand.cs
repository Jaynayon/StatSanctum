using MediatR;

namespace StatSanctum.API.Queries.Users
{
    public class ValidateUserCommand<User> : IRequest<(int, string?)>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
