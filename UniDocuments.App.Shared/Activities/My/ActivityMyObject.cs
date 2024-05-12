using UniDocuments.App.Shared.Activities.Shared;

namespace UniDocuments.App.Shared.Activities.My;

public class ActivityMyObject
{
    public Guid Id { get; set; }
    public string CreatorFirstName { get; set; } = null!;
    public string CreatorLastName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ActivityDocumentObject? Document { get; set; }
}