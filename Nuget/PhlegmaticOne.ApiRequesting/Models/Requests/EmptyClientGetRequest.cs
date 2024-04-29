namespace PhlegmaticOne.ApiRequesting.Models.Requests;

public abstract class EmptyClientGetRequest<TResponse> : ClientGetRequest<object, TResponse>
{
    protected EmptyClientGetRequest() : base(default!)
    {
        IsEmpty = true;
    }

    public sealed override string BuildQueryString()
    {
        return string.Empty;
    }
}