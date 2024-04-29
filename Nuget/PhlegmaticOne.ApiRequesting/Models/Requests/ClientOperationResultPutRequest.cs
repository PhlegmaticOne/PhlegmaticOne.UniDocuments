using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.ApiRequesting.Models.Requests;

public abstract class ClientOperationResultPutRequest<TRequest> : ClientPutRequest<TRequest, OperationResult>
{
    protected ClientOperationResultPutRequest(TRequest requestData) : base(requestData)
    {
    }
}