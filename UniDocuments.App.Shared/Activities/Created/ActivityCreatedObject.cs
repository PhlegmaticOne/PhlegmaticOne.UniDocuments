namespace UniDocuments.App.Shared.Activities.Created;

public class ActivityCreatedObject
{
    public Guid Id { get; set; }
    public string CreatorFirstName { get; set; } = null!;
    public string CreatorLastName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int StudentsCount { get; set; }
    public int DocumentsCount { get; set; }
}