namespace UniDocuments.App.Shared.Activities;

public class CreateActivityObject
{
    public Guid TeacherId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}