using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.Cache;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Extensions;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Documents.Uploading;

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
    private const string UploadDocumentInternalError = "UploadDocument.InternalError";
    
    private readonly IDocumentsStorage _documentsStorage;
    private readonly IStreamContentReader _streamContentReader;
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly IUniDocumentsCache _documentsCache;
    private readonly IDocumentMapper _documentMapper;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CommandUploadDocumentHandler> _logger;

    public CommandUploadDocumentHandler(
        IDocumentsStorage documentsStorage, 
        IStreamContentReader streamContentReader,
        IFingerprintsProvider fingerprintsProvider,
        IUniDocumentsCache documentsCache,
        IDocumentMapper documentMapper,
        ApplicationDbContext dbContext,
        ILogger<CommandUploadDocumentHandler> logger)
    {
        _documentsStorage = documentsStorage;
        _streamContentReader = streamContentReader;
        _fingerprintsProvider = fingerprintsProvider;
        _documentsCache = documentsCache;
        _documentMapper = documentMapper;
        _dbContext = dbContext;
        _logger = logger;
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
            _logger.LogCritical(e, UploadDocumentInternalError);
            return OperationResult.Failed<Guid>(UploadDocumentInternalError, e.Message);
        }
    }

    private async Task<OperationResult<Guid>> ExecuteAsync(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        if (await CanLoadToActivityAsync(request.ActivityId, request.ProfileId, cancellationToken))
        {
            return OperationResult.Failed<Guid>("Can't load document to activity");
        }
        
        var content = await _streamContentReader.ReadAsync(request.DocumentStream, cancellationToken);

        var newDocument = await CreateDocumentAsync(request, content, cancellationToken);

        var documentId = newDocument.Entity.Id;

        var document = new UniDocument(documentId, content, request.FileName);
        
        var fileId = await SaveDocumentFileAsync(documentId, request, cancellationToken);

        newDocument.Property(x => x.StudyDocumentFileId).CurrentValue = fileId;

        await CalculateFingerprintAsync(newDocument, document, cancellationToken);

        CacheDocument(document);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Successful(documentId);
    }

    private async Task<bool> CanLoadToActivityAsync(Guid activityId, Guid studentId, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Set<StudyActivity>()
            .Where(x => x.Id == activityId)
            .Select(x => new
            {
                Students = x.Students.Select(s => s.Id),
                x.EndDate
            })
            .ToListAsync(cancellationToken);

        if (data.Count == 0)
        {
            return false;
        }

        var activityData = data[0];

        return DateTime.UtcNow < activityData.EndDate && activityData.Students.Contains(studentId);
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
            ValuableParagraphsCount = content.ParagraphsCount,
        }, cancellationToken);
    }

    private void CacheDocument(UniDocument document)
    {
        _documentsCache.Cache(document);
        _documentMapper.AddDocument(document, document.Name);
    }

    private async Task CalculateFingerprintAsync(
        EntityEntry<StudyDocument> newDocument, UniDocument document, CancellationToken cancellationToken)
    {
        var fingerprint = await _fingerprintsProvider.ComputeAsync(document, cancellationToken).ConfigureAwait(false);
        newDocument.Property(x => x.Fingerprint).CurrentValue = JsonConvert.SerializeObject(fingerprint.Entries);
    }

    private Task<Guid> SaveDocumentFileAsync(Guid id, CommandUploadDocument request, CancellationToken cancellationToken)
    {
        var stream = request.DocumentStream;
        var saveRequest = new DocumentSaveRequest(id, request.FileName, stream);
        return _documentsStorage.SaveAsync(saveRequest, cancellationToken);
    }
}