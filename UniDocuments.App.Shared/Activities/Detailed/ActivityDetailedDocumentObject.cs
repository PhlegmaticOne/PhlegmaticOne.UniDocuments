namespace UniDocuments.App.Shared.Activities.Detailed;

public class ActivityDetailedDocumentObject
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime DateLoaded { get; set; }
}