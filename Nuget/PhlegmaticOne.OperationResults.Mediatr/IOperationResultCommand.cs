using MediatR;

namespace PhlegmaticOne.OperationResults.Mediatr;

public interface IOperationResultCommand : IRequest<OperationResult>
{
}