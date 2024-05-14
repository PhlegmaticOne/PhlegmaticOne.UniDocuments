using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Domain.Services.Documents;
using UniDocuments.Text.Domain.Services.Cache;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Extensions;

namespace UniDocuments.App.Application.Documents.Loading.Commands;

public class CommandUploadDocument : IOperationResultCommand
{
    public Guid ProfileId { get; set; }
    public Stream DocumentStream { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public Guid ActivityId { get; set; }

    public DocumentSaveRequest ToSaveRequest()
    {
        return new DocumentSaveRequest(ProfileId, ActivityId, DocumentStream, FileName);
    }
}

public class CommandUploadDocumentHandler : IOperationResultCommandHandler<CommandUploadDocument>
{
    private const string UploadDocumentInternalError = "UploadDocument.InternalError";
    
    private readonly IUniDocumentsCache _documentsCache;
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentSaveProvider _documentSaveProvider;
    private readonly ILogger<CommandUploadDocumentHandler> _logger;

    public CommandUploadDocumentHandler(
        IUniDocumentsCache documentsCache,
        IDocumentMapper documentMapper,
        IDocumentSaveProvider documentSaveProvider,
        ILogger<CommandUploadDocumentHandler> logger)
    {
        _documentsCache = documentsCache;
        _documentMapper = documentMapper;
        _documentSaveProvider = documentSaveProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _documentSaveProvider.SaveAsync(request.ToSaveRequest(), cancellationToken);
            _documentsCache.Cache(document);
            _documentMapper.AddDocument(document);
            return OperationResult.Successful(document.Id);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, UploadDocumentInternalError);
            return OperationResult.Failed<Guid>(UploadDocumentInternalError, e.Message);
        }
    }
}