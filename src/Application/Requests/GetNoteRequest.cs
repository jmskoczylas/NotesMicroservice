using FluentResults;
using Infrastructure.Models;
using MediatR;

namespace Application.Requests
{
    /// <summary>
    /// Request object for getting a note.
    /// </summary>
    public class GetNoteRequest : IRequest<Result<NoteDto>>
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public GetNoteRequest(int id)
        {
            this.Id = id;
        }
    }
}
