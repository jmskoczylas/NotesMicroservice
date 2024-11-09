using Application.DTOs;
using FluentResults;
using MediatR;

namespace Application.Requests
{
    /// <summary>
    /// Request object for updating a note.
    /// </summary>
    public class UpdateNoteRequest : IRequest<Result<NoteDto>>
    {
        /// <summary>
        /// Gets the note to update.
        /// </summary>
        public NoteDto Note { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNoteRequest"/> class.
        /// </summary>
        /// <param name="note">The note to update.</param>
        public UpdateNoteRequest(NoteDto note)
        {
            this.Note = note;
        }
    }
}
