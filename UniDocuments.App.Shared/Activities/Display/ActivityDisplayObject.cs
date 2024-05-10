namespace UniDocuments.App.Shared.Activities.Display;

public class ActivityDisplayObject
{
    public Guid Id { get; set; }
    public string Creator { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int StudentsCount { get; set; }
    public int DocumentsCount { get; set; }
    public bool IsExpired { get; set; }
}