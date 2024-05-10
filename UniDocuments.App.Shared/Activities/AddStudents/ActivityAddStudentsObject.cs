namespace UniDocuments.App.Shared.Activities.AddStudents;

public class ActivityAddStudentsObject
{
    public Guid ActivityId { get; set; }
    public List<Guid> Students { get; set; } = null!;
}