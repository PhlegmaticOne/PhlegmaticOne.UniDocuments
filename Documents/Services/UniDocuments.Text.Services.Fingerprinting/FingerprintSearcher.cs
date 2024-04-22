using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting;

public class FingerprintSearcher : IFingerprintSearcher
{
    private readonly IFingerprintContainer _fingerprintContainer;
    private readonly IDocumentMapper _documentMapper;

    public FingerprintSearcher(IFingerprintContainer fingerprintContainer, IDocumentMapper documentMapper)
    {
        _fingerprintContainer = fingerprintContainer;
        _documentMapper = documentMapper;
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
        var documentFingerprint = _fingerprintContainer.Get(documentId);
        var result = new DocumentSearchData[n];

        foreach (var (id, fingerprint) in allFingerprints)
        {
            if (documentId == id)
            {
                continue;
            }

            var similarity = documentFingerprint.CalculateJaccard(fingerprint);
            var documentName = _documentMapper.GetDocumentData(id)!.Name;

            if (currentIndex < topN)
            {
                result[currentIndex] = new DocumentSearchData(id, documentName, similarity);
                currentIndex++;
                continue;
            }

            if (!isSorted)
            {
                Array.Sort(result, (a, b) => a.Similarity.CompareTo(b.Similarity));
                isSorted = true;
            }

            for (var i = topN - 1; i >= 0; i--)
            {
                if (similarity > result[i].Similarity)
                {
                    result[i] = new DocumentSearchData(id, documentName, similarity);
                }
            }
        }

        return result;
    }
}