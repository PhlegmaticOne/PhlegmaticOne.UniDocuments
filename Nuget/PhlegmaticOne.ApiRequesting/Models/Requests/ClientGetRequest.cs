namespace PhlegmaticOne.ApiRequesting.Models.Requests;

public abstract class ClientGetRequest<TRequest, TResponse> : ClientQueryBuildableRequest<TRequest, TResponse>
{
    protected ClientGetRequest(TRequest requestData) : base(requestData)
    {
        RequestData = requestData;
    }

    public bool IsEmpty { get; protected set; }
}