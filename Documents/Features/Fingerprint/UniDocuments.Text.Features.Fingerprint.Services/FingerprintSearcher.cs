using UniDocuments.Text.Domain.Services.Common;
using UniDocuments.Text.Domain.Services.Searching.Response;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public class FingerprintSearcher : IFingerprintSearcher
{
    private class FingerprintComputeData
    {
        public static FingerprintComputeData Empty => new(Guid.Empty, 0);
        public FingerprintComputeData(Guid documentId, int sharedCount)
        {
            DocumentId = documentId;
            SharedCount = sharedCount;
        }

        public Guid DocumentId { get; }
        public int SharedCount { get; }
    }
    
    private readonly IFingerprintsContainer _fingerprintsContainer;
    private readonly IDocumentsMapper _documentsMapper;

    public FingerprintSearcher(IFingerprintsContainer fingerprintsContainer, IDocumentsMapper documentsMapper)
    {
        _fingerprintsContainer = fingerprintsContainer;
        _documentsMapper = documentsMapper;
    }
    
    public Task<List<DocumentSearchData>> SearchTopAsync(Guid documentId, int topN)
    {
        return Task.Run(() => ComputeTopFingerprints(documentId, topN));
    }

    private List<DocumentSearchData> ComputeTopFingerprints(Guid documentId, int topN)
    {
        var currentMaxIndex = 0;
        var topNFingerprints = new FingerprintComputeData[topN];
        var fingerprint = _fingerprintsContainer.Get(documentId)!;
        Array.Fill(topNFingerprints, FingerprintComputeData.Empty);

        foreach (var documentFingerprintData in _fingerprintsContainer.GetAll())
        {
            var other = documentFingerprintData.Value!;
            
            if (fingerprint == other)
            {
                continue;
            }

            var count = fingerprint.GetSharedPrintsCount(other);

            if (currentMaxIndex < topN)
            {
                if (count > topNFingerprints[currentMaxIndex].SharedCount)
                {
                    topNFingerprints[currentMaxIndex] = new FingerprintComputeData(documentFingerprintData.Key, count);
                    currentMaxIndex++;
                }
            }
            else
            {
                if (count > topNFingerprints[^1].SharedCount)
                {
                    ShiftArray(topNFingerprints, new FingerprintComputeData(documentFingerprintData.Key, count));
                }
            }
        }

        return topNFingerprints.Where(x => x.SharedCount != 0).Select(x => new DocumentSearchData
        {
            Id = x.DocumentId,
            Name = _documentsMapper.GetDocumentName(x.DocumentId)
        }).ToList();
    }

    private static void ShiftArray(FingerprintComputeData[] originalArray, FingerprintComputeData newLast)
    {
        for (var i = 1; i < originalArray.Length; i++)
        {
            originalArray[i - 1] = originalArray[i];
        }

        originalArray[^1] = newLast;
    }
}