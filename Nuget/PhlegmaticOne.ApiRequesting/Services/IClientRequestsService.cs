using PhlegmaticOne.ApiRequesting.Models;
using PhlegmaticOne.ApiRequesting.Models.Requests;

namespace PhlegmaticOne.ApiRequesting.Services;

public interface IClientRequestsService
{
    Task<ServerResponse<TResponse>> PostAsync<TRequest, TResponse>(
        ClientPostRequest<TRequest, TResponse> request, string? jwtToken = null);

    Task<ServerResponse<TResponse>> DeleteAsync<TRequest, TResponse>(
        ClientDeleteRequest<TRequest, TResponse> request, string? jwtToken = null);

    Task<ServerResponse<TResponse>> PutAsync<TRequest, TResponse>(
        ClientPutRequest<TRequest, TResponse> request, string? jwtToken = null);

    Task<ServerResponse<TResponse>> GetAsync<TRequest, TResponse>(
        ClientGetRequest<TRequest, TResponse> request, string? jwtToken = null);
}