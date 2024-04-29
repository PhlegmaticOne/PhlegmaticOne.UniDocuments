namespace PhlegmaticOne.ApiRequesting.Models.Requests;

public abstract class ClientPostRequest<TRequest, TResponse> : ClientRequest<TRequest, TResponse>
{
    protected ClientPostRequest(TRequest requestData) : base(requestData)
    {
    }
}