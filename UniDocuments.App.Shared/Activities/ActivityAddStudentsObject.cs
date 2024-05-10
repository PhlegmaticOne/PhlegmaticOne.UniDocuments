namespace UniDocuments.App.Shared.Activities;

public class ActivityAddStudentsObject
{
    public Guid ActivityId { get; set; }
    public List<Guid> Students { get; set; } = null!;
}