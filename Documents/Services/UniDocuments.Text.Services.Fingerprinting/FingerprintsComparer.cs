using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintsComparer : IFingerprintsComparer
{
    private readonly IFingerprintOptionsProvider _optionsProvider;

    public FingerprintsComparer(IFingerprintOptionsProvider optionsProvider)
    {
        _optionsProvider = optionsProvider;
    }
    
    public List<ReportDocumentData> Compare(TextFingerprint source, List<TextFingerprint> other)
    {
        var options = _optionsProvider.GetOptions();
        var result = new List<ReportDocumentData>();

        foreach (var fingerprint in other)
        {
            var id = fingerprint.DocumentId;
            var similarity = fingerprint.CalculateJaccard(source);

            if (similarity >= options.Baseline)
            {
                result.Add(new ReportDocumentData());
            }
        }

        return result;
    }
}