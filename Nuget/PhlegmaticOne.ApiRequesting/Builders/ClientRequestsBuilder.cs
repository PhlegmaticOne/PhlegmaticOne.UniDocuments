using PhlegmaticOne.ApiRequesting.Models.Requests;

namespace PhlegmaticOne.ApiRequesting.Builders;

public class ClientRequestsBuilder
{
    private readonly Dictionary<Type, string> _requestUrls = new();

    public void ConfigureRequest<T>(string requestUrl) where T : ClientRequest
    {
        _requestUrls.Add(typeof(T), requestUrl);
    }

    internal Dictionary<Type, string> Build()
    {
        return _requestUrls;
    }
}