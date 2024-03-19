namespace PhlegmaticOne.OperationResults;

[Serializable]
public class OperationResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static OperationResult Success => new()
    {
        IsSuccess = true
    };

    public static OperationResult<T> Successful<T>(T result)
    {
        return new()
        {
            IsSuccess = true,
            ErrorMessage = null,
            Result = result
        };
    }

    public static OperationResult<T> Failed<T>(string? errorMessage = null)
    {
        return new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage ?? "Operation error",
            Result = default
        };
    }

    public static OperationResult Failed(string? errorMessage = null)
    {
        return new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage ?? "Operation error"
        };
    }
}

[Serializable]
public class OperationResult<T> : OperationResult
{
    public T? Result { get; init; }
}