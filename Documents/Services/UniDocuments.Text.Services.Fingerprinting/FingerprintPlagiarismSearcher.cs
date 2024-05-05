using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintPlagiarismSearcher : IFingerprintPlagiarismSearcher
{
    private readonly IFingerprintContainer _fingerprintContainer;

    public FingerprintPlagiarismSearcher(IFingerprintContainer fingerprintContainer)
    {
        _fingerprintContainer = fingerprintContainer;
    }
    
    public Task<DocumentSearchData[]> SearchTopAsync(Guid documentId, int topN, CancellationToken cancellationToken)
    {
        return Task.Run(() => ComputeTopFingerprints(documentId, topN), cancellationToken);
    }

    private DocumentSearchData[] ComputeTopFingerprints(Guid documentId, int topN)
    {
        var isSorted = false;
        var currentIndex = 0;
        var allFingerprints = _fingerprintContainer.GetAll();
        var n = allFingerprints.Count - 1 < topN ? allFingerprints.Count - 1 : topN;
        var documentFingerprint = _fingerprintContainer.Get(documentId)!;
        var result = new DocumentSearchData[n];

        foreach (var (id, fingerprint) in allFingerprints)
        {
            if (documentId == id)
            {
                continue;
            }

            var similarity = documentFingerprint.CalculateJaccard(fingerprint);

            if (currentIndex < topN)
            {
                result[currentIndex] = new DocumentSearchData(id, similarity);
                currentIndex++;
                continue;
            }

            SortTopFingerprints(ref isSorted, result);
            RebuildTopFingerprints(topN, similarity, id, result);
        }

        return result;
    }

    private void SortTopFingerprints(ref bool isSorted, DocumentSearchData[] result)
    {
        if (!isSorted)
        {
            Array.Sort(result, (a, b) => a.Similarity.CompareTo(b.Similarity));
            isSorted = true;
        }
    }

    private static void RebuildTopFingerprints(int topN, double similarity, Guid id, DocumentSearchData[] result)
    {
        for (var i = topN - 1; i >= 0; i--)
        {
            if (similarity > result[i].Similarity)
            {
                result[i] = new DocumentSearchData(id, similarity);
            }
        }
    }
}