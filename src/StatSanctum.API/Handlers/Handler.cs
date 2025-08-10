using MediatR;
using StatSanctum.Helpers;
using StatSanctum.Repositories;

namespace StatSanctum.Handlers
{
    public class Handler<T> :
    IRequestHandler<GetByIdQuery<T>, T>,
    IRequestHandler<GetAllQuery<T>, IEnumerable<T>>,
    IRequestHandler<CreateCommand<T>, T>,
    IRequestHandler<UpdateCommand<T>, T>,
    IRequestHandler<DeleteCommand<T>, bool>
    where T : Base
    {
        private readonly IRepository<T> _repository;

        public Handler(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> Handle(GetByIdQuery<T> request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }

        public async Task<IEnumerable<T>> Handle(GetAllQuery<T> request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> Handle(CreateCommand<T> request, CancellationToken cancellationToken)
        {
            return await _repository.CreateAsync(request.Entity);
        }

        public async Task<T> Handle(UpdateCommand<T> request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateAsync(request.Entity);
        }

        public async Task<bool> Handle(DeleteCommand<T> request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteByIdAsync(request.Id);
        }
    }
}
