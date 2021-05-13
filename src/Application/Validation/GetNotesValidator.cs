using Application.Requests;
using FluentValidation;

namespace Application.Validation
{
    /// <summary>
    /// Validator for getting paged notes.
    /// </summary>
    public class GetNotesValidator : AbstractValidator<GetNotesRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotesValidator"/> class.
        /// </summary>
        public GetNotesValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
