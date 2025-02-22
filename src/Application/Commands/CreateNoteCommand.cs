using Application.DTOs;
using FluentResults;
using MediatR;

namespace Application.Commands
{
    /// <summary>
    /// Request object for creating a note.
    /// </summary>
    public class CreateNoteCommand : IRequest<Result<NoteDto>>
    {
        /// <summary>
        /// Gets the note to create.
        /// </summary>
        public NoteDto Note { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNoteCommand"/> class.
        /// </summary>
        /// <param name="note">The note to create.</param>
        public CreateNoteCommand(NoteDto note)
        {
            this.Note = note;
        }
    }
}
