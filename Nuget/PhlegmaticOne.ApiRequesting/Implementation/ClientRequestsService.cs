using System.Net.Http.Headers;
using System.Net.Http.Json;
using PhlegmaticOne.ApiRequesting.Models;
using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.ApiRequesting.Implementation;

public class ClientRequestsService : IClientRequestsService
{
    private const string PreQueryPart = "/?";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _httpClientName;
    private readonly Dictionary<Type, string> _requestUrls;

    public ClientRequestsService(IHttpClientFactory httpClientFactory,
        Dictionary<Type, string> requestUrls,
        string httpClientName)
    {
        _httpClientFactory = httpClientFactory;
        _httpClientName = httpClientName;
        _requestUrls = requestUrls;
    }

    public async Task<ServerResponse<TResponse>> PostAsync<TRequest, TResponse>(
        ClientPostRequest<TRequest, TResponse> postRequest, string? jwtToken = null)
    {
        var requestUrl = GetRequestUrl(postRequest);
        var httpClient = CreateHttpClientWithToken(jwtToken);

        try
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(requestUrl, postRequest.RequestData);
            return await GetServerResponse<TResponse>(httpResponseMessage);
        }
        catch (HttpRequestException httpRequestException)
        {
            return ServerResponse.FromError<TResponse>(httpRequestException.StatusCode, httpRequestException.Message);
        }
    }

    public async Task<ServerResponse<TResponse>> DeleteAsync<TRequest, TResponse>(
        ClientDeleteRequest<TRequest, TResponse> request, string? jwtToken = null)
    {
        var requestUrl = BuildRequestQuery(request);
        var httpClient = CreateHttpClientWithToken(jwtToken);

        HttpResponseMessage httpResponseMessage;
        try
        {
            httpResponseMessage = await httpClient.DeleteAsync(requestUrl);
        }
        catch (HttpRequestException httpRequestException)
        {
            return ServerResponse.FromError<TResponse>(httpRequestException.StatusCode, httpRequestException.Message);
        }

        return await GetServerResponse<TResponse>(httpResponseMessage);
    }

    public async Task<ServerResponse<TResponse>> PutAsync<TRequest, TResponse>(
        ClientPutRequest<TRequest, TResponse> request, string? jwtToken = null)
    {
        var requestUrl = GetRequestUrl(request);
        var httpClient = CreateHttpClientWithToken(jwtToken);

        HttpResponseMessage httpResponseMessage;
        try
        {
            httpResponseMessage = await httpClient.PutAsJsonAsync(requestUrl, request.RequestData);
        }
        catch (HttpRequestException httpRequestException)
        {
            return ServerResponse.FromError<TResponse>(httpRequestException.StatusCode, httpRequestException.Message);
        }

        return await GetServerResponse<TResponse>(httpResponseMessage);
    }

    public async Task<ServerResponse<TResponse>> GetAsync<TRequest, TResponse>(
        ClientGetRequest<TRequest, TResponse> getRequest, string? jwtToken = null)
    {
        var requestUrl = BuildGetQuery(getRequest);
        var httpClient = CreateHttpClientWithToken(jwtToken);

        HttpResponseMessage httpResponseMessage;
        try
        {
            httpResponseMessage = await httpClient.GetAsync(requestUrl);
        }
        catch (HttpRequestException httpRequestException)
        {
            return ServerResponse.FromError<TResponse>(httpRequestException.StatusCode, httpRequestException.Message);
        }

        return await GetServerResponse<TResponse>(httpResponseMessage);
    }

    private HttpClient CreateHttpClientWithToken(string? jwtToken)
    {
        var httpClient = _httpClientFactory.CreateClient(_httpClientName);
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(Constants.BearerAuthenticationSchemeName, jwtToken);
        return httpClient;
    }

    private string BuildRequestQuery<TRequest, TResponse>(ClientQueryBuildableRequest<TRequest, TResponse> request)
    {
        return string.Concat(GetRequestUrl(request), PreQueryPart, request.BuildQueryString());
    }

    private string BuildGetQuery<TRequest, TResponse>(ClientGetRequest<TRequest, TResponse> clientGetRequest)
    {
        return clientGetRequest.IsEmpty ? GetRequestUrl(clientGetRequest) : BuildRequestQuery(clientGetRequest);
    }

    private string GetRequestUrl(ClientRequest clientRequest)
    {
        return _requestUrls[clientRequest.GetType()];
    }

    private static async Task<ServerResponse<TResponse>> GetServerResponse<TResponse>(
        HttpResponseMessage httpResponseMessage)
    {
        var httpStatusCode = httpResponseMessage.StatusCode;
        var reasonPhrase = httpResponseMessage.ReasonPhrase;

        if (httpResponseMessage.IsSuccessStatusCode == false)
            return ServerResponse.FromError<TResponse>(httpStatusCode, reasonPhrase);

        var result = await httpResponseMessage.Content.ReadFromJsonAsync<OperationResult<TResponse>>();
        return ServerResponse.FromSuccess(result!, httpStatusCode, reasonPhrase);
    }
}