using Application.Requests;
using FluentValidation;

namespace Application.Validation
{
    /// <summary>
    /// Validation rules for getting note by id.
    /// </summary>
    public class GetNoteValidator : AbstractValidator<GetNoteRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNoteValidator"/> class.
        /// </summary>
        public GetNoteValidator()
        {
            RuleFor(x => x.Id).LessThanOrEqualTo(int.MaxValue).GreaterThan(0);
        }
    }
}
