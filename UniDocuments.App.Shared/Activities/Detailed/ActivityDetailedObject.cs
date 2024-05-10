namespace UniDocuments.App.Shared.Activities.Detailed;

public class ActivityDetailedObject
{
    public Guid Id { get; set; }
    public string Creator { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsExpired { get; set; }
    public List<ActivityDetailedStudentObject> Students { get; set; } = null!;
}