using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.ApiRequesting.Models.Requests;

public abstract class ClientOperationResultPostRequest<TRequest> : ClientPostRequest<TRequest, OperationResult>
{
    protected ClientOperationResultPostRequest(TRequest requestData) : base(requestData)
    {
    }
}