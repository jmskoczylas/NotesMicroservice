using FluentResults;
using Infrastructure.Models;
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
        public UpdateNoteDto Note { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNoteRequest"/> class.
        /// </summary>
        /// <param name="note">The note to update.</param>
        public UpdateNoteRequest(UpdateNoteDto note)
        {
            this.Note = note;
        }
    }
}
