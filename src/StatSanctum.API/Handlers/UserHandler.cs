using MediatR;
using StatSanctum.API.Queries.Users;
using StatSanctum.API.Repositories;
using StatSanctum.Entities;

namespace StatSanctum.API.Handlers
{
    public class UserHandler : 
        IRequestHandler<CreateUserCommand<User>, User>,
        IRequestHandler<ValidateUserCommand<User>, (int, string?)>
    {
        private readonly IUserRepository _userRepository;

        public UserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(CreateUserCommand<User> request, CancellationToken cancellationToken)
        {
            return await _userRepository.CreateUserAsync(request.Entity);
        }

        public async Task<(int, string?)> Handle(ValidateUserCommand<User> request, CancellationToken cancellationToken)
        {
            return await _userRepository.ValidateUserCredentials(request.Username, request.Password);
        }
    }
}
