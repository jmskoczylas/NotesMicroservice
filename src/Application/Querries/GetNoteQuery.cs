using Application.DTOs;
using FluentResults;
using MediatR;

namespace Application.Querries
{
    /// <summary>
    /// Request object for getting a note.
    /// </summary>
    public class GetNoteQuery : IRequest<Result<NoteDto>>
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNoteQuery" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public GetNoteQuery(int id)
        {
            Id = id;
        }
    }
}
