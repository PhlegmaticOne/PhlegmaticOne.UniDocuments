namespace UniDocuments.App.Shared.Documents;

public class DocumentSearchResultEntry
{
    public Guid DocumentId { get; set; }
    public string DocumentName { get; set; } = null!;
    public DateTime DateLoaded { get; set; }
    public string StudentFirstName { get; set; } = null!;
    public string StudentLastName { get; set; } = null!;
}