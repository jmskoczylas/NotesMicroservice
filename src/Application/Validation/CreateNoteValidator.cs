using Application.Requests;
using FluentValidation;

namespace Application.Validation
{
    /// <summary>
    /// Validator for creating a note.
    /// </summary>
    public class CreateNoteValidator : AbstractValidator<CreateNoteRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNoteValidator"/> class.
        /// </summary>
        public CreateNoteValidator()
        {
            RuleFor(x => x.Note.Title).Length(1,50).OverridePropertyName(nameof(CreateNoteRequest.Note.Title));
            RuleFor(x => x.Note.Text).Length(1,200).OverridePropertyName(nameof(CreateNoteRequest.Note.Text));
            RuleFor(x => x.Note.Id).Equal(0).OverridePropertyName(nameof(CreateNoteRequest.Note.Id));
        }
    }
}
