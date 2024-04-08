﻿using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services.FileStorage;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Documents;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint;
using UniDocuments.Text.Features.Fingerprint.Services;

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
    private readonly ApplicationDbContext _dbContext;

    public CommandUploadDocumentHandler(
        IFileStorage fileStorage, 
        IStreamContentReader streamContentReader,
        IFingerprintComputer fingerprintComputer,
        IUniDocumentsService uniDocumentsService,
        ApplicationDbContext dbContext)
    {
        _fileStorage = fileStorage;
        _streamContentReader = streamContentReader;
        _fingerprintComputer = fingerprintComputer;
        _uniDocumentsService = uniDocumentsService;
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult> Handle(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        var documentId = await SaveDocumentAsync(request, cancellationToken);
        var content = await _streamContentReader.ReadAsync(request.DocumentStream, cancellationToken);
        var rawText = content.ToRawText();
        var fingerprint = await _fingerprintComputer.ComputeAsync(documentId, rawText, cancellationToken);

        var document = new UniDocument(documentId, new UniDocumentFeatureFingerprint(fingerprint));
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
        return saveResponse.FileId;
    }
}