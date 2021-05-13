using Application.Requests;
using FluentValidation;

namespace Application.Validation
{
    /// <summary>
    /// Validator for deleting a note.
    /// </summary>
    public class DeleteNoteValidator : AbstractValidator<DeleteNoteRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteNoteValidator"/> class.
        /// </summary>
        public DeleteNoteValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
