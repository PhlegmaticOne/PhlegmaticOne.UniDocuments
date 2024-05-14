using Newtonsoft.Json;

namespace PhlegmaticOne.OperationResults;

[Serializable]
public class OperationResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorData { get; init; }
    public string? ErrorCode { get; init; }

    public virtual object? GetResult()
    {
        return null;
    }

    public static OperationResult<T> Successful<T>(T result)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            ErrorData = null,
            Result = result
        };
    }

    public static OperationResult Failed(string? errorCode, object errorData)
    {
        return Failed(errorCode, JsonConvert.SerializeObject(errorData));
    }

    public static OperationResult Failed<T>(string? errorCode, object errorData)
    {
        return Failed<T>(errorCode, JsonConvert.SerializeObject(errorData));
    }
        
    public static OperationResult<T> Failed<T>(string? errorCode = null, string? errorData = null, T? result = default)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            ErrorData = errorData ?? string.Empty,
            Result = result,
            ErrorCode = errorCode ?? string.Empty
        };
    }

    public static OperationResult Failed(string? errorCode = null, string? errorData = null)
    {
        return new OperationResult
        {
            IsSuccess = false,
            ErrorData = errorData ?? string.Empty,
            ErrorCode = errorCode ?? string.Empty
        };
    }
}

[Serializable]
public class OperationResult<T> : OperationResult
{
    public T? Result { get; init; }

    public override object? GetResult()
    {
        return Result;
    }
}