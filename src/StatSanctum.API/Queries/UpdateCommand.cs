using MediatR;
using StatSanctum.Helpers;

namespace StatSanctum.Handlers
{
    public class UpdateCommand<T> : IRequest<T> where T : Base
    {
        public T Entity { get; set; }
    }
}
