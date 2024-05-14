namespace UniDocuments.Text.Domain.Services.Reports.Models;

public class ReportDocumentData
{
    public string Name { get; set; } = null!;
    public DateTime DateLoaded { get; set; }
    public string StudentFirstName { get; set; } = null!;
    public string StudentLastName { get; set; } = null!;
    public double Similarity { get; set; }
}