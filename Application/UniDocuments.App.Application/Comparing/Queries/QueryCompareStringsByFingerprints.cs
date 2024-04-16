using MediatR;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint.Models;
using UniDocuments.Text.Features.Fingerprint.Services;

namespace UniDocuments.App.Application.Comparing.Queries;

public record FingerprintEntry(DocumentFingerprint Fingerprint, double JaccardSimilarity);
public record FingerprintsResponse(DocumentFingerprint Fingerprint, List<FingerprintEntry> CompareEntries);

public class QueryCompareStringsByFingerprints : IRequest<FingerprintsResponse>
{
    public string Text { get; }
    public string[] OtherTexts { get; }

    public QueryCompareStringsByFingerprints(string text, string[] otherTexts)
    {
        Text = text;
        OtherTexts = otherTexts;
    }
}

public class QueryCompareByFingerprints : IRequestHandler<QueryCompareStringsByFingerprints, FingerprintsResponse>
{
    private readonly IFingerprintAlgorithm _fingerprintAlgorithm;

    public QueryCompareByFingerprints(IFingerprintAlgorithm fingerprintAlgorithm)
    {
        _fingerprintAlgorithm = fingerprintAlgorithm;
    }
    
    public Task<FingerprintsResponse> Handle(QueryCompareStringsByFingerprints request, CancellationToken cancellationToken)
    {
        return Task.Run(() => CalculateFingerprints(request), cancellationToken);
    }

    private FingerprintsResponse CalculateFingerprints(QueryCompareStringsByFingerprints request)
    {
        var data = StreamContentReadResult.FromString(request.Text);
        var fingerprint = _fingerprintAlgorithm.Fingerprint(data);

        var selectFingerprintQuery =
            from otherText in request.OtherTexts
            let content = StreamContentReadResult.FromString(otherText) 
            let otherFingerprint = _fingerprintAlgorithm.Fingerprint(content) 
            let jaccardSimilarity = fingerprint.CalculateJaccard(otherFingerprint)
            select new FingerprintEntry(otherFingerprint, jaccardSimilarity);

        return new FingerprintsResponse(fingerprint, selectFingerprintQuery.ToList());
    }
}