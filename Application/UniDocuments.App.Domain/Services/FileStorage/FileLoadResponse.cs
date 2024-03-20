namespace UniDocuments.App.Domain.Services.FileStorage;

public record FileLoadResponse(Guid FileId, string FileName, Stream? FileStream)
{
    public static FileLoadResponse NoContent()
    {
        return new FileLoadResponse(Guid.Empty, string.Empty, null);
    }
}
