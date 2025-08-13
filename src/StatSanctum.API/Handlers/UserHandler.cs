using MediatR;
using StatSanctum.API.Enums;
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
            Func<string, string, Task<(int, string?)>> validateUser;

            switch (request.Method)
            {
                case AuthenticationMethod.Google:
                    validateUser = _userRepository.ValidateUserCredentials;
                    break;
                case AuthenticationMethod.Manual:
                    validateUser = _userRepository.ValidateUserCredentials;
                    break;
                default:
                    throw new NotSupportedException($"Validation method '{request.Method}' is not supported.");
            }

            return await validateUser(request.Username, request.Password);
        }
    }
}
