using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Extensions;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintsComparer : IFingerprintsComparer
{
    private readonly IFingerprintOptionsProvider _optionsProvider;
    private readonly IDocumentMapper _documentMapper;

    public FingerprintsComparer(IFingerprintOptionsProvider optionsProvider, IDocumentMapper documentMapper)
    {
        _optionsProvider = optionsProvider;
        _documentMapper = documentMapper;
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
                var documentName = _documentMapper.GetDocumentData(id)!.Name;
                result.Add(new ReportDocumentData(id, documentName, similarity));
            }
        }

        return result;
    }
}