using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting;

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

    public Task<TextFingerprint> ComputeAsync(UniDocument document, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var options = _optionsProvider.GetOptions();
            var fingerprint = _algorithm.Fingerprint(document, options);
            _container.AddOrReplace(document.Id, fingerprint);
            return fingerprint;
        }, cancellationToken);
    }

    public Task<TextFingerprint> GetForDocumentAsync(UniDocument content, CancellationToken cancellationToken)
    {
        if (content.Id == Guid.Empty)
        {
            var options = _optionsProvider.GetOptions();
            return Task.Run(() => _algorithm.Fingerprint(content, options), cancellationToken);
        }

        return GetForDocumentAsync(content.Id, cancellationToken);
    }

    public async Task<TextFingerprint> GetForDocumentAsync(Guid documentId, CancellationToken cancellationToken)
    {
        var cached = _container.Get(documentId);

        if (cached is not null)
        {
            return cached;
        }

        var fingerprintText = await _dbContext.Set<StudyDocument>()
            .Where(x => x.Id == documentId)
            .Select(x => x.Fingerprint)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return CreateFingerprint(documentId, fingerprintText!);
    }

    public async Task<List<TextFingerprint>> GetForDocumentsAsync(
        IEnumerable<Guid> documentIds, CancellationToken cancellationToken)
    {
        var finding = new List<Guid>(documentIds);
        var result = new List<TextFingerprint>();

        for (var i = finding.Count - 1; i >= 0; i--)
        {
            var saved = _container.Get(finding[i]);

            if (saved is not null)
            {
                result.Add(saved);
                finding.RemoveAt(i);
            }
        }

        var fingerprintsData = await _dbContext.Set<StudyDocument>()
            .Where(x => finding.Contains(x.Id))
            .Select(x => new
            {
                x.Id,
                x.Fingerprint,
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        foreach (var fingerprintData in fingerprintsData)
        {
            result.Add(CreateFingerprint(fingerprintData.Id, fingerprintData.Fingerprint));
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