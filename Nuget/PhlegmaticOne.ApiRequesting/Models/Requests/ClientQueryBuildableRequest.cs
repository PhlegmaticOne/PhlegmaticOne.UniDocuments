namespace PhlegmaticOne.ApiRequesting.Models.Requests;

public abstract class ClientQueryBuildableRequest<TRequest, TResponse> : ClientRequest<TRequest, TResponse>
{
    protected ClientQueryBuildableRequest(TRequest requestData) : base(requestData)
    {
    }

    public abstract string BuildQueryString();

    protected string WithOneQueryParameter(GetRequestQueryParameter getRequestQueryParameter)
    {
        return getRequestQueryParameter.BuildQueryPart();
    }

    protected string WithManyQueryParameters(params GetRequestQueryParameter[] queryParameters)
    {
        return string.Join('&', queryParameters.Select(x => x.BuildQueryPart()));
    }
}