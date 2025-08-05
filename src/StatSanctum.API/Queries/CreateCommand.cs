using MediatR;
using StatSanctum.Helpers;

namespace StatSanctum.Handlers
{
    public class CreateCommand<T> : IRequest<T> where T : Base 
    {
        public T Entity { get; set; }
    }
}
