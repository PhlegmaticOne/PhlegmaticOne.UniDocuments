namespace UniDocuments.Text.Domain.Providers.Reports.Data.Models;

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