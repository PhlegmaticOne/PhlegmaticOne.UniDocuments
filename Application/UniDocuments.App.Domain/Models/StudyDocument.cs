using Newtonsoft.Json;
using UniDocuments.App.Domain.Models.Base;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;

namespace UniDocuments.App.Domain.Models;

public class StudyDocument : EntityBase
{
    public string Name { get; set; } = null!;
    public DateTime DateLoaded { get; set; }
    public string Fingerprint { get; set; } = null!;
    public int ValuableParagraphsCount { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid ActivityId { get; set; }
    public StudyActivity Activity { get; set; } = null!;

    public void UpdateFingerprint(TextFingerprint fingerprint)
    {
        Fingerprint = JsonConvert.SerializeObject(fingerprint.Entries);
    }
}