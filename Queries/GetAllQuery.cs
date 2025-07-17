using MediatR;
using StatSanctum.Helpers;

namespace StatSanctum.Handlers
{
    public class GetAllQuery<T> : IRequest<IEnumerable<T>> where T : Base
    {
    }
}