namespace PhlegmaticOne.ApiRequesting.Models.Requests;

public abstract class ClientRequest
{
}

public abstract class ClientRequest<TRequest, TResponse> : ClientRequest
{
    protected ClientRequest(TRequest requestData)
    {
        RequestData = requestData;
    }

    public TRequest RequestData { get; set; }
}