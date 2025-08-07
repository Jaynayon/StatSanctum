using MediatR;

namespace StatSanctum.API.Queries.Users
{
    public class CreateUserCommand<User> : IRequest<User>
    {
        public User Entity { get; set; }
    }
}
