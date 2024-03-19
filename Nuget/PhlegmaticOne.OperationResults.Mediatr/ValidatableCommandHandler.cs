using FluentValidation;
using FluentValidation.Results;

namespace PhlegmaticOne.OperationResults.Mediatr;

public abstract class ValidatableCommandHandler<TCommand> : IOperationResultCommandHandler<TCommand>
    where TCommand : IOperationResultCommand
{
    private readonly IValidator<TCommand> _commandValidator;

    protected ValidatableCommandHandler(IValidator<TCommand> commandValidator) => _commandValidator = commandValidator;

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _commandValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return OnValidationTriggered(validationResult);
        }

        return await HandleValidCommand(request, cancellationToken);
    }

    protected abstract Task<OperationResult> HandleValidCommand(TCommand request, CancellationToken cancellationToken);

    protected virtual OperationResult OnValidationTriggered(ValidationResult validationResult) =>
        OperationResult.Failed(validationResult.ToString());
}