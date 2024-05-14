using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Domain.Services.Documents;

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
    
    private readonly IDocumentSaveProvider _documentSaveProvider;
    private readonly ILogger<CommandUploadDocumentHandler> _logger;

    public CommandUploadDocumentHandler(
        IDocumentSaveProvider documentSaveProvider,
        ILogger<CommandUploadDocumentHandler> logger)
    {
        _documentSaveProvider = documentSaveProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _documentSaveProvider.SaveAsync(request.ToSaveRequest(), cancellationToken);
            return OperationResult.Successful(document.Id);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, UploadDocumentInternalError);
            return OperationResult.Failed<Guid>(UploadDocumentInternalError, e.Message);
        }
    }
}