using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviors
{
    using ValidationContext = ValidationContext<object>;

    /// <summary>
    /// Represents a validation behaviour for the mediator pipeline.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IMapper _mapper;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBehavior{T,T}"/> class.
        /// </summary>
        /// <param name="validators">
        /// A list of validators.
        /// </param>
        /// <param name="mapper">
        /// An instance of <see cref="IMapper"/>.
        /// </param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IMapper mapper)
        {
            this._validators = validators ?? throw new NullReferenceException(nameof(validators));
            this._mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        /// <summary>
        /// Handlers a mediator request for the validation behaviour.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="next">
        /// The next stage in the pipeline.
        /// </param>
        /// <returns>The response.</returns>
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (request is not IRequest<ResultBase> || !this._validators.Any())
            {
                return await next();
            }
            
            var context = new ValidationContext(request);
            var validationResults = await Task.WhenAll(
                this._validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );
            var failures = validationResults.SelectMany(r => r.Errors)
                                                                .Where(f => f != null)
                                                                .ToList();
            if (failures.Count == 0)
            {
                return await next();
            }
            ResultBase result = Result.Merge(
                failures.Select(
                            failure => Result.Fail(
                                new Error(failure.ErrorMessage)
                            )
                        )
                        .Cast<ResultBase>()
                        .ToArray()
            );
            return await Task.FromResult(this._mapper.Map<ResultBase, TResponse>(result));
        }
    }
}