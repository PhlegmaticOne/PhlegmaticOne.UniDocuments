namespace PhlegmaticOne.OperationResults;

[Serializable]
public class OperationResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public string? ErrorCode { get; init; }

    public static OperationResult Success => new()
    {
        IsSuccess = true
    };

    public virtual object? GetResult()
    {
        return null;
    }

    public static OperationResult<T> Successful<T>(T result)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            ErrorMessage = null,
            Result = result
        };
    }

    public static OperationResult<T> Failed<T>(string? errorCode = null, string? errorMessage = null, T? result = default)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage ?? string.Empty,
            Result = result,
            ErrorCode = errorCode ?? string.Empty
        };
    }

    public static OperationResult Failed(string? errorCode = null, string? errorMessage = null)
    {
        return new OperationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage ?? string.Empty,
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