using FluentResults;
using MediatR;

namespace Application.Commands
{
    /// <summary>
    /// Request object for deleting a note.
    /// </summary>
    public class DeleteNoteCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteNoteCommand"/> class.
        /// </summary>
        /// <param name="id">The note identifier.</param>
        public DeleteNoteCommand(int id)
        {
            this.Id = id;
        }
    }
}
