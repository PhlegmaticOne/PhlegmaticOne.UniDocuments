using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;

namespace UniDocuments.Text.Application.Fingerprinting;

public class FingerprintsProvider : IFingerprintsProvider
{
    private readonly IFingerprintAlgorithm _algorithm;
    private readonly IFingerprintOptionsProvider _optionsProvider;
    private readonly IFingerprintContainer _container;
    private readonly ApplicationDbContext _dbContext;

    public FingerprintsProvider(
        IFingerprintAlgorithm algorithm, 
        IFingerprintOptionsProvider optionsProvider, 
        IFingerprintContainer container,
        ApplicationDbContext dbContext)
    {
        _algorithm = algorithm;
        _optionsProvider = optionsProvider;
        _container = container;
        _dbContext = dbContext;
    }

    public TextFingerprint Compute(UniDocument document)
    {
        var fingerprint = Get(document);
        _container.AddOrReplace(document.Id, fingerprint);
        return fingerprint;
    }

    public TextFingerprint Get(UniDocument content)
    {
        var options = _optionsProvider.GetOptions();
        return _algorithm.Fingerprint(content, options);
    }

    public FingerprintCompareResult Compare(TextFingerprint a, TextFingerprint b)
    {
        var options = _optionsProvider.GetOptions();
        var similarity = a.CalculateJaccard(b);
        return new FingerprintCompareResult(similarity, similarity > options.Baseline);
    }

    public async Task<Dictionary<Guid, TextFingerprint>> GetForDocumentsAsync(
        IEnumerable<Guid> documentIds, CancellationToken cancellationToken)
    {
        var finding = new List<Guid>(documentIds);
        var result = new Dictionary<Guid, TextFingerprint>();

        for (var i = finding.Count - 1; i >= 0; i--)
        {
            var saved = _container.Get(finding[i]);

            if (saved is not null)
            {
                _container.AddOrReplace(finding[i], saved);
                result.Add(finding[i], saved);
                finding.RemoveAt(i);
            }
        }

        if (finding.Count == 0)
        {
            return result;
        }

        var fingerprintsQuery = _dbContext.Set<StudyDocument>()
            .Where(x => finding.Contains(x.Id))
            .Select(x => new
            {
                x.Id,
                x.Fingerprint,
            })
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false);
        
        await foreach (var fingerprintData in fingerprintsQuery)
        {
            result.Add(fingerprintData.Id, CreateFingerprint(fingerprintData.Id, fingerprintData.Fingerprint));
        }

        return result;
    }
    
    private TextFingerprint CreateFingerprint(Guid documentId, string fingerprintText)
    {
        var fingerprintData = JsonConvert.DeserializeObject<HashSet<uint>>(fingerprintText);
        var fingerprint = new TextFingerprint(documentId, fingerprintData!);
        _container.AddOrReplace(documentId, fingerprint);
        return fingerprint;
    }
}