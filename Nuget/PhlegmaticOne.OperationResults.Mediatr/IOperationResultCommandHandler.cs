using MediatR;

namespace PhlegmaticOne.OperationResults.Mediatr;

public interface IOperationResultCommandHandler<in TCommand> : IRequestHandler<TCommand, OperationResult>
    where TCommand : IOperationResultCommand
{
}