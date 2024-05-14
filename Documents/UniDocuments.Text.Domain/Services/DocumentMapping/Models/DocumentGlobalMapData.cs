namespace UniDocuments.Text.Domain.Services.DocumentMapping.Models;

public class DocumentGlobalMapData
{
    public DocumentGlobalMapData(Guid id, int globalFirstParagraphId)
    {
        Id = id;
        GlobalFirstParagraphId = globalFirstParagraphId;
    }

    public Guid Id { get; }
    public int GlobalFirstParagraphId { get; }

    public int GetLocalParagraphId(int globalParagraphId)
    {
        return globalParagraphId - GlobalFirstParagraphId;
    }
}