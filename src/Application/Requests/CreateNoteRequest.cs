using FluentResults;
using Infrastructure.Models;
using MediatR;

namespace Application.Requests
{
    /// <summary>
    /// Request object for creating a note.
    /// </summary>
    public class CreateNoteRequest : IRequest<Result<NoteDto>>
    {
        /// <summary>
        /// Gets the note to create.
        /// </summary>
        public CreateNoteDto Note { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNoteRequest"/> class.
        /// </summary>
        /// <param name="note">The note to create.</param>
        public CreateNoteRequest(CreateNoteDto note)
        {
            this.Note = note;
        }
    }
}
