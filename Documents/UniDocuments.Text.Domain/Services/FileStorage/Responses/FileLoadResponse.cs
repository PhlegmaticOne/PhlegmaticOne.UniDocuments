namespace UniDocuments.Text.Domain.Services.FileStorage.Responses;

public record FileLoadResponse(Guid FileId, string FileName, Stream? FileStream)
{
    public static FileLoadResponse NoContent()
    {
        return new FileLoadResponse(Guid.Empty, string.Empty, null);
    }
}
