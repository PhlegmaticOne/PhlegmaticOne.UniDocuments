using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Domain.Services.Documents;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Services.Documents;

public class DocumentSaveProvider : IDocumentSaveProvider
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStreamContentReader _streamContentReader;
    private readonly ITimeProvider _timeProvider;
    private readonly IDocumentsStorage _documentsStorage;
    private readonly IFingerprintsProvider _fingerprintsProvider;

    public DocumentSaveProvider(
        ApplicationDbContext dbContext,
        IStreamContentReader streamContentReader,
        ITimeProvider timeProvider,
        IDocumentsStorage documentsStorage,
        IFingerprintsProvider fingerprintsProvider)
    {
        _dbContext = dbContext;
        _streamContentReader = streamContentReader;
        _timeProvider = timeProvider;
        _documentsStorage = documentsStorage;
        _fingerprintsProvider = fingerprintsProvider;
    }

    public async Task<UniDocument> SaveAsync(DocumentSaveRequest request, CancellationToken cancellationToken)
    {
        var content = await _streamContentReader.ReadAsync(request.DocumentStream, cancellationToken);
        var newDocument = await CreateDocumentAsync(request, content, cancellationToken);
        var documentId = newDocument.Entity.Id;
        var document = new UniDocument(documentId, content, request.FileName);

        await Task.WhenAll(
            CalculateFingerprintAsync(newDocument, document, cancellationToken),
            SaveDocumentFileAsync(documentId, request, cancellationToken));

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return document;
    }

    private Task CalculateFingerprintAsync(
        EntityEntry<StudyDocument> newDocument, UniDocument document, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var fingerprint = _fingerprintsProvider.Compute(document).Entries;
            newDocument.Property(x => x.Fingerprint).CurrentValue = JsonConvert.SerializeObject(fingerprint);
        }, cancellationToken);
    }
    
    private Task<Guid> SaveDocumentFileAsync(
        Guid id, DocumentSaveRequest request, CancellationToken cancellationToken)
    {
        var stream = request.DocumentStream;
        var saveRequest = new StorageSaveRequest(id, request.FileName, stream);
        return _documentsStorage.SaveAsync(saveRequest, cancellationToken);
    }
    
    private async Task<EntityEntry<StudyDocument>> CreateDocumentAsync(
        DocumentSaveRequest request, UniDocumentContent content, CancellationToken cancellationToken)
    {
        var documents = _dbContext.Set<StudyDocument>();
        
        var existing = await documents
            .Where(x => x.ActivityId == request.ActivityId && x.StudentId == request.ProfileId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is not null)
        {
            return UpdateExisting(existing, request, content);
        }

        return await CreateNew(request, content, cancellationToken);
    }

    private ValueTask<EntityEntry<StudyDocument>> CreateNew(
        DocumentSaveRequest request, UniDocumentContent content, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StudyDocument>().AddAsync(new StudyDocument
        {
            ActivityId = request.ActivityId,
            StudentId = request.ProfileId,
            DateLoaded = _timeProvider.Now,
            Name = request.FileName,
            ValuableParagraphsCount = content.ParagraphsCount,
        }, cancellationToken);
    }

    private EntityEntry<StudyDocument> UpdateExisting(
        StudyDocument existing, DocumentSaveRequest request, UniDocumentContent content)
    {
        existing.DateLoaded = _timeProvider.Now;
        existing.Name = request.FileName;
        existing.ValuableParagraphsCount = content.ParagraphsCount;
        return _dbContext.Set<StudyDocument>().Update(existing);
    }
}