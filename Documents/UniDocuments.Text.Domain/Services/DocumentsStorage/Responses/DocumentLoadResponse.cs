namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public record DocumentLoadResponse(string Name, Stream? Stream)
{
    public static DocumentLoadResponse NoContent()
    {
        return new DocumentLoadResponse(string.Empty, null);
    }

    public ValueTask DisposeAsync()
    {
        return Stream?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}
