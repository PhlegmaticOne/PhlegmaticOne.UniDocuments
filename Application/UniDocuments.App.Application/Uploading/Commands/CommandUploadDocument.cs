using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Documents;
using UniDocuments.Text.Domain.Services.FileStorage;
using UniDocuments.Text.Domain.Services.FileStorage.Requests;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Uploading.Commands;

public class CommandUploadDocument : IdentityOperationResultCommand
{
    public Stream DocumentStream { get; }
    public Guid ActivityId { get; }

    public CommandUploadDocument(Guid profileId, Guid activityId, Stream documentStream) : base(profileId)
    {
        ActivityId = activityId;
        DocumentStream = documentStream;
    }
}

public class CommandUploadDocumentHandler : IOperationResultCommandHandler<CommandUploadDocument>
{
    private readonly IFileStorage _fileStorage;
    private readonly IStreamContentReader _streamContentReader;
    private readonly IFingerprintComputer _fingerprintComputer;
    private readonly IUniDocumentsService _uniDocumentsService;
    private readonly IDocumentMapper _documentMapper;
    private readonly ApplicationDbContext _dbContext;

    public CommandUploadDocumentHandler(
        IFileStorage fileStorage, 
        IStreamContentReader streamContentReader,
        IFingerprintComputer fingerprintComputer,
        IUniDocumentsService uniDocumentsService,
        IDocumentMapper documentMapper,
        ApplicationDbContext dbContext)
    {
        _fileStorage = fileStorage;
        _streamContentReader = streamContentReader;
        _fingerprintComputer = fingerprintComputer;
        _uniDocumentsService = uniDocumentsService;
        _documentMapper = documentMapper;
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult> Handle(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        var documentId = await SaveDocumentAsync(request, cancellationToken);
        var content = await _streamContentReader.ReadAsync(request.DocumentStream, cancellationToken);
        var fingerprint = await _fingerprintComputer.ComputeAsync(documentId, content, cancellationToken);

        var document = new UniDocument(documentId);
        await _uniDocumentsService.SaveAsync(document, cancellationToken);

        await _dbContext.Set<StudyDocument>().AddAsync(new StudyDocument
        {
            Id = documentId,
            ActivityId = request.ActivityId,
            StudentId = request.ProfileId,
            DateLoaded = DateTime.UtcNow,
            WinnowingData = fingerprint.ToByteArray()
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return OperationResult.Successful(documentId);
    }

    private async Task<Guid> SaveDocumentAsync(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        var stream = request.DocumentStream;
        var saveRequest = new FileSaveRequest("test", stream);
        var saveResponse = await _fileStorage.SaveAsync(saveRequest, cancellationToken);
        _documentMapper.AddMap(saveResponse.FileId, saveRequest.FileName);
        return saveResponse.FileId;
    }
}