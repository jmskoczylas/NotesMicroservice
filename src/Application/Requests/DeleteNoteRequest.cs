using FluentResults;
using MediatR;

namespace Application.Requests
{
    /// <summary>
    /// Request object for deleting a note.
    /// </summary>
    public class DeleteNoteRequest : IRequest<Result<int>>
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteNoteRequest"/> class.
        /// </summary>
        /// <param name="id">The note identifier.</param>
        public DeleteNoteRequest(int id)
        {
            this.Id = id;
        }
    }
}
