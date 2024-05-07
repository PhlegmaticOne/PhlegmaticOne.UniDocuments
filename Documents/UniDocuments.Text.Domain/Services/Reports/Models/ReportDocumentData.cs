namespace UniDocuments.Text.Domain.Services.Reports.Models;

public class ReportDocumentData
{
    public Guid Id { get; }
    public string Name { get; }
    public double Similarity { get; }

    public ReportDocumentData(Guid id, string name, double similarity)
    {
        Id = id;
        Name = name;
        Similarity = similarity;
    }
}