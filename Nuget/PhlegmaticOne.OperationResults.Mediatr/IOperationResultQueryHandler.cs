using MediatR;

namespace PhlegmaticOne.OperationResults.Mediatr;

public interface IOperationResultQueryHandler<in TRequest, TResponse> : 
    IRequestHandler<TRequest, OperationResult<TResponse>> where TRequest : IOperationResultQuery<TResponse>
{
}