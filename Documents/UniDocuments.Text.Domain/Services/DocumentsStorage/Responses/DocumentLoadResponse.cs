namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

public record DocumentLoadResponse(Guid Id, string Name, Stream? Stream)
{
    public static DocumentLoadResponse NoContent()
    {
        return new DocumentLoadResponse(Guid.Empty, string.Empty, null);
    }
}
