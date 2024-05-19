using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Documents;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Documents.Training;

public class CommandRebuildFingerprints : IOperationResultCommand { }

public class CommandRebuildFingerprintsHandler : IOperationResultCommandHandler<CommandRebuildFingerprints>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStreamContentReader _streamContentReader;
    private readonly IFingerprintsProvider _fingerprintsProvider;

    public CommandRebuildFingerprintsHandler(
        ApplicationDbContext dbContext, 
        IStreamContentReader streamContentReader,
        IFingerprintsProvider fingerprintsProvider)
    {
        _dbContext = dbContext;
        _streamContentReader = streamContentReader;
        _fingerprintsProvider = fingerprintsProvider;
    }
    
    public async Task<OperationResult> Handle(CommandRebuildFingerprints request, CancellationToken cancellationToken)
    {
        var timer = Stopwatch.StartNew();

        try
        {
            var count = await UpdateFingerprintsAsync(cancellationToken);
            
            return OperationResult.Successful(new FingerprintsUpdateObject
            {
                Count = count,
                Time = timer.Elapsed
            });
        }
        catch (Exception e)
        {
            return OperationResult.Failed(e.Message, new FingerprintsUpdateObject
            {
                Count = 0,
                Time = timer.Elapsed
            });
        }
    }

    private async Task<int> UpdateFingerprintsAsync(CancellationToken cancellationToken)
    {
        var query = _dbContext.Set<StudyDocumentFile>()
            .Select(x => new
            {
                x.StudyDocument,
                x.Content
            })
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false);

        var count = 0;
        
        await foreach (var file in query)
        {
            using var stream = new MemoryStream(file.Content);
            var content = await _streamContentReader.ReadAsync(stream, cancellationToken);
            var fingerprint = _fingerprintsProvider.Get(UniDocument.FromContent(content));
            file.StudyDocument.UpdateFingerprint(fingerprint);
            await _dbContext.SaveChangesAsync(cancellationToken);
            count++;
        }

        return count;
    }
}