using FluentValidation;
using FluentValidation.Results;

namespace PhlegmaticOne.OperationResults.Mediatr;

public abstract class ValidatableQueryHandler<TRequest, TResponse> :
    IOperationResultQueryHandler<TRequest, TResponse>
    where TRequest : IOperationResultQuery<TResponse>
{
    private readonly IValidator<TRequest> _queryValidator;

    protected ValidatableQueryHandler(IValidator<TRequest> queryValidator) => _queryValidator = queryValidator;

    public async Task<OperationResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _queryValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return OnValidationTriggered(validationResult);
        }

        return await HandleValidQuery(request, cancellationToken);
    }

    protected abstract Task<OperationResult<TResponse>> HandleValidQuery(TRequest request, CancellationToken cancellationToken);

    protected virtual OperationResult<TResponse> OnValidationTriggered(ValidationResult validationResult) =>
        OperationResult.Failed<TResponse>(validationResult.ToString());
}