using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.DocumentMapping.Initializers;

public class DocumentMappingInitializer : IDocumentMappingInitializer
{
    private readonly IDocumentMapper _documentMapper;
    private readonly IFingerprintContainer _fingerprintContainer;
    private readonly ApplicationDbContext _dbContext;

    public DocumentMappingInitializer(
        IDocumentMapper documentMapper,
        IFingerprintContainer fingerprintContainer,
        ApplicationDbContext dbContext)
    {
        _documentMapper = documentMapper;
        _fingerprintContainer = fingerprintContainer;
        _dbContext = dbContext;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var query = _dbContext
            .Set<StudyDocument>()
            .Select(x => new 
            { 
                x.Id, 
                x.Name,
                x.Fingerprint,
                x.ValuableParagraphsCount
            })
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken);

        await foreach (var data in query)
        {
            _documentMapper.AddDocument(data.Id, data.ValuableParagraphsCount, data.Name);
            _fingerprintContainer.AddOrReplace(data.Id, CreateFingerprint(data.Fingerprint));
        }
    }

    private TextFingerprint CreateFingerprint(string data)
    {
        var set = JsonConvert.DeserializeObject<HashSet<uint>>(data);
        return new TextFingerprint(set!);
    }
}