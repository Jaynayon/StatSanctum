using MediatR;
using StatSanctum.Helpers;

namespace StatSanctum.Handlers
{
    public class DeleteCommand<T> : IRequest<bool> where T : Base
    {
        public int Id { get; set; }
    }
}
