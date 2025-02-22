using Application.DTOs;
using FluentResults;
using MediatR;

namespace Application.Commands
{
    /// <summary>
    /// Request object for updating a note.
    /// </summary>
    public class UpdateNoteCommand : IRequest<Result<NoteDto>>
    {
        /// <summary>
        /// Gets the note to update.
        /// </summary>
        public NoteDto Note { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNoteCommand"/> class.
        /// </summary>
        /// <param name="note">The note to update.</param>
        public UpdateNoteCommand(NoteDto note)
        {
            this.Note = note;
        }
    }
}
