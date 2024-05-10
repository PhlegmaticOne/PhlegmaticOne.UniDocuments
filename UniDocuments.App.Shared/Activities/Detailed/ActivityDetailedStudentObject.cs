namespace UniDocuments.App.Shared.Activities.Detailed;

public class ActivityDetailedStudentObject
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public List<ActivityDetailedDocumentObject> Documents { get; set; } = null!;
}