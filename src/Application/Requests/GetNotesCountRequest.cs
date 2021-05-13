using FluentResults;
using MediatR;

namespace Application.Requests
{
    /// <summary>
    /// Request object for getting notes count.
    /// </summary>
    public class GetNotesCountRequest : IRequest<Result<int>>
    {

    }
}
