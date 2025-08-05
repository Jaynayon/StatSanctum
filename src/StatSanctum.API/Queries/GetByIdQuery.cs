using MediatR;
using StatSanctum.Helpers;

namespace StatSanctum.Handlers
{
    public class GetByIdQuery<T> : IRequest<T> where T : Base
    {
        public int Id { get; set; }
    }
}
