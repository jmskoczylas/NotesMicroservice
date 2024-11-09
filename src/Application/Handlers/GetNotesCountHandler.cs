using Application.Requests;
using Domain.Interfaces;
using FluentResults;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    /// <summary>
    /// Handler for getting notes count.
    /// </summary>
    public class GetNotesCountHandler : IRequestHandler<GetNotesCountRequest, Result<int>>
    {
        private readonly INoteRepository _noteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotesCountHandler"/> class.
        /// </summary>
        /// <param name="noteRepository">An instance of <see cref="INoteRepository"/> to interact with the notes.</param>
        /// <exception cref="ArgumentNullException">noteRepository</exception>
        public GetNotesCountHandler(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Result<int>> Handle(GetNotesCountRequest request, CancellationToken cancellationToken) => await this._noteRepository.GetCountAsync();
    }
}
