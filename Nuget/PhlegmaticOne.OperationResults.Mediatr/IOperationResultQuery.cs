using MediatR;

namespace PhlegmaticOne.OperationResults.Mediatr;

public interface IOperationResultQuery<T> : IRequest<OperationResult<T>>
{
}