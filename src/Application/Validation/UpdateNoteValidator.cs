using Application.Requests;
using FluentValidation;

namespace Application.Validation
{
    /// <summary>
    /// Validator for updating a note.
    /// </summary>
    public class UpdateNoteValidator : AbstractValidator<UpdateNoteRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNoteValidator"/> class.
        /// </summary>
        public UpdateNoteValidator()
        {
            RuleFor(x => x.Note.Title).Length(1,50).OverridePropertyName(nameof(UpdateNoteRequest.Note.Title));
            RuleFor(x => x.Note.Id).GreaterThan(0).OverridePropertyName(nameof(UpdateNoteRequest.Note.Id));
        }
    }
}
