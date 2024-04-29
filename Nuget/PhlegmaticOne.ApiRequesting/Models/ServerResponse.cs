using System.Net;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.ApiRequesting.Models;

[Serializable]
public class ServerResponse
{
    public HttpStatusCode? StatusCode { get; init; }
    public string ReasonPhrase { get; init; } = string.Empty;
    public bool IsSuccess { get; init; }
    public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;

    public static ServerResponse<T> FromError<T>(HttpStatusCode? statusCode, string? reasonPhrase)
    {
        return new()
        {
            ReasonPhrase = reasonPhrase ?? string.Empty,
            StatusCode = statusCode,
            IsSuccess = false,
            OperationResult = default
        };
    }

    public static ServerResponse<T> FromSuccess<T>(OperationResult<T> result, HttpStatusCode? statusCode,
        string? reasonPhrase)
    {
        return new()
        {
            ReasonPhrase = reasonPhrase ?? string.Empty,
            StatusCode = statusCode,
            IsSuccess = true,
            OperationResult = result
        };
    }

    public override string ToString()
    {
        return StatusCode is not null ? StatusCode + ": " + ReasonPhrase : ReasonPhrase;
    }
}

[Serializable]
public class ServerResponse<TResponse> : ServerResponse
{
    public OperationResult<TResponse>? OperationResult { get; internal set; }

    public TResponse? GetData()
    {
        return OperationResult is null ? default : OperationResult.Result;
    }
}