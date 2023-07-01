using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace RestaurantSimulation.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validator is null)
            {
                return await next();
            }

            ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            List<Error> errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage))
                .ToList();

            return (dynamic)errors;
        }
    }
}
