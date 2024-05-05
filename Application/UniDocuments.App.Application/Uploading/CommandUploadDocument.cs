using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Cache;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Uploading;

public class CommandUploadDocument : IdentityOperationResultCommand
{
    public Stream DocumentStream { get; }
    public string FileName { get; }
    public Guid ActivityId { get; }

    public CommandUploadDocument(Guid profileId, Guid activityId, Stream documentStream, string fileName) : base(profileId)
    {
        ActivityId = activityId;
        DocumentStream = documentStream;
        FileName = fileName;
    }
}

public class CommandUploadDocumentHandler : IOperationResultCommandHandler<CommandUploadDocument>
{
    private readonly IDocumentsStorage _documentsStorage;
    private readonly IStreamContentReader _streamContentReader;
    private readonly IFingerprintComputer _fingerprintComputer;
    private readonly IUniDocumentsCache _documentsCache;
    private readonly IDocumentMapper _documentMapper;
    private readonly ApplicationDbContext _dbContext;

    public CommandUploadDocumentHandler(
        IDocumentsStorage documentsStorage, 
        IStreamContentReader streamContentReader,
        IFingerprintComputer fingerprintComputer,
        IUniDocumentsCache documentsCache,
        IDocumentMapper documentMapper,
        ApplicationDbContext dbContext)
    {
        _documentsStorage = documentsStorage;
        _streamContentReader = streamContentReader;
        _fingerprintComputer = fingerprintComputer;
        _documentsCache = documentsCache;
        _documentMapper = documentMapper;
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult> Handle(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await ExecuteAsync(request, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult.Failed("UploadDocument.InternalError", e.Message);
        }
    }

    private async Task<Guid> ExecuteAsync(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        var content = await _streamContentReader.ReadAsync(request.DocumentStream, cancellationToken);
        
        var newDocument = await CreateDocumentAsync(request, content, cancellationToken);
        
        var documentId = newDocument.Entity.Id;

        await CalculateFingerprintAsync(newDocument, content, cancellationToken);

        await SaveDocumentFileAsync(documentId, request, cancellationToken);
        
        CacheDocument(newDocument, content);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return documentId;
    }

    private ValueTask<EntityEntry<StudyDocument>> CreateDocumentAsync(
        CommandUploadDocument request, UniDocumentContent content, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StudyDocument>().AddAsync(new StudyDocument
        {
            ActivityId = request.ActivityId,
            StudentId = request.ProfileId,
            DateLoaded = DateTime.UtcNow,
            Name = request.FileName,
            ValuableParagraphsCount = content.ParagraphsCount
        }, cancellationToken);
    }

    private void CacheDocument(EntityEntry<StudyDocument> newDocument, UniDocumentContent content)
    {
        var documentId = newDocument.Entity.Id;
        var name = newDocument.Entity.Name;
        var document = new UniDocument(documentId, content);
        _documentsCache.Cache(document);
        _documentMapper.AddDocument(document, name);
    }

    private async Task CalculateFingerprintAsync(
        EntityEntry<StudyDocument> newDocument, UniDocumentContent content, CancellationToken cancellationToken)
    {
        var documentId = newDocument.Entity.Id;
        var fingerprint = await _fingerprintComputer.ComputeAsync(documentId, content, cancellationToken);
        newDocument.Property(x => x.Fingerprint).CurrentValue =
            await Task.Run(() => JsonConvert.SerializeObject(fingerprint), cancellationToken);
    }

    private async Task SaveDocumentFileAsync(Guid id, CommandUploadDocument request, CancellationToken cancellationToken)
    {
        var stream = request.DocumentStream;
        var saveRequest = new DocumentSaveRequest(id, request.FileName, stream);
        await _documentsStorage.SaveAsync(saveRequest, cancellationToken);
    }
}