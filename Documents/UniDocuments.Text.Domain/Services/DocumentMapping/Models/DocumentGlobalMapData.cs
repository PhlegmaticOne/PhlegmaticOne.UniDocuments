namespace UniDocuments.Text.Domain.Services.DocumentMapping.Models;

public class DocumentGlobalMapData
{
    public DocumentGlobalMapData(Guid id, string name, int globalFirstParagraphId)
    {
        Id = id;
        Name = name;
        GlobalFirstParagraphId = globalFirstParagraphId;
    }

    public Guid Id { get; }
    public string Name { get; }
    public int GlobalFirstParagraphId { get; }
}