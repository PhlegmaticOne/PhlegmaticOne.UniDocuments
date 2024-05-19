using UniDocuments.Text.Domain.Services.Fingerprinting.Models;

namespace UniDocuments.Text.Domain.Services.Reports.Models;

public class ReportLoadData
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime DateLoaded { get; set; }
    public string StudentFirstName { get; set; } = null!;
    public string StudentLastName { get; set; } = null!;
    public TextFingerprint Fingerprint { get; set; } = null!;
    public UniDocument Document { get; set; } = null!;

    public ReportDocumentData ToDocumentData(double similarity)
    {
        return new ReportDocumentData
        {
            DateLoaded = DateLoaded,
            StudentFirstName = StudentFirstName,
            StudentLastName = StudentLastName,
            Name = Name,
            Similarity = similarity
        };
    }
}