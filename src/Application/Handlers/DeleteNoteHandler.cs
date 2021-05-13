using Application.Requests;
using FluentResults;
using Infrastructure.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    /// <summary>
    /// Handler for deleting a note.
    /// </summary>
    public class DeleteNoteHandler : IRequestHandler<DeleteNoteRequest, Result<int>>
    {
        private readonly INoteRepository _noteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteNoteHandler"/> class.
        /// </summary>
        /// <param name="noteRepository">An instance of <see cref="INoteRepository"/> to interact with the notes.</param>
        /// <exception cref="ArgumentNullException">noteRepository</exception>
        public DeleteNoteHandler(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Result<int>> Handle(DeleteNoteRequest request, CancellationToken cancellationToken) => await _noteRepository.DeleteAsync(request.Id);
    }
}
