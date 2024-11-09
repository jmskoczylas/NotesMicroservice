using Application.DTOs;
using FluentResults;
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
        public NoteDto Note { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNoteRequest"/> class.
        /// </summary>
        /// <param name="note">The note to create.</param>
        public CreateNoteRequest(NoteDto note)
        {
            this.Note = note;
        }
    }
}
