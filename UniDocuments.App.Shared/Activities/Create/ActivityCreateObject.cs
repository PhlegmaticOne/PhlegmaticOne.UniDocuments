namespace UniDocuments.App.Shared.Activities.Create;

public class ActivityCreateObject
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<string> Students { get; set; } = null!;
}