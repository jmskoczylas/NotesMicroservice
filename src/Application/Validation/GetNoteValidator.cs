using Application.Querries;
using FluentValidation;

namespace Application.Validation
{
    /// <summary>
    /// Validation rules for getting note by id.
    /// </summary>
    public class GetNoteValidator : AbstractValidator<GetNoteQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNoteValidator"/> class.
        /// </summary>
        public GetNoteValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).LessThanOrEqualTo(int.MaxValue);
        }
    }
}
