using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Services.Fingerprinting.Initializers;

public class FingerprintContainerInitializer : IFingerprintContainerInitializer
{
    private readonly IFingerprintContainer _fingerprintContainer;
    private readonly ApplicationDbContext _dbContext;

    public FingerprintContainerInitializer(IFingerprintContainer fingerprintContainer, ApplicationDbContext dbContext)
    {
        _fingerprintContainer = fingerprintContainer;
        _dbContext = dbContext;
    }
    
    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        var query = _dbContext.Set<StudyDocument>().Select(x => new
        {
            x.Id,
            x.WinnowingData
        });

        foreach (var fingerprintData in query)
        {
            var fingerprint = TextFingerprint.FromBytes(fingerprintData.WinnowingData);
            _fingerprintContainer.AddOrReplace(fingerprintData.Id, fingerprint);
        }
        
        return Task.CompletedTask;
    }
}