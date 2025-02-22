using Application.Querries;
using FluentValidation;

namespace Application.Validation
{
    /// <summary>
    /// Validator for getting paged notes.
    /// </summary>
    public class GetNotesValidator : AbstractValidator<GetNotesQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotesValidator"/> class.
        /// </summary>
        public GetNotesValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}
