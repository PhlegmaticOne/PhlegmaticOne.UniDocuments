namespace PhlegmaticOne.UniDocuments.Documents.Core.Algorithms;

public class PlagiarismResultError : IPlagiarismResult
{
    private PlagiarismResultError(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public static PlagiarismResultError Default => new PlagiarismResultError("Something went wrong...");

    public static PlagiarismResultError FromMessage(string message)
    {
        return new PlagiarismResultError(message);
    } 

    public string ErrorMessage { get; }

    public object GetRawValue()
    {
        return ErrorMessage;
    }
}
