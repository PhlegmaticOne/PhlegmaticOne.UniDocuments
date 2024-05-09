using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Documents.Plagiarism.Fingerprinting;

public class QueryCalculateFingerprintDocument : IOperationResultQuery<TextFingerprint>
{
    public Stream DocumentStream { get; set; } = null!;
    public string DocumentName { get; set; } = null!;
}

public class QueryCalculateFingerprintDocumentHandler : 
    IOperationResultQueryHandler<QueryCalculateFingerprintDocument, TextFingerprint>
{
    private const string? CalculateFingerprintDocumentInternalError = "CalculateFingerprintDocument.InternalError";
    
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly IStreamContentReader _streamContentReader;
    private readonly ILogger<QueryCalculateFingerprintDocumentHandler> _logger;

    public QueryCalculateFingerprintDocumentHandler(
        IFingerprintsProvider fingerprintsProvider,
        IStreamContentReader streamContentReader,
        ILogger<QueryCalculateFingerprintDocumentHandler> logger)
    {
        _fingerprintsProvider = fingerprintsProvider;
        _streamContentReader = streamContentReader;
        _logger = logger;
    }
    
    public async Task<OperationResult<TextFingerprint>> Handle(QueryCalculateFingerprintDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var content = await _streamContentReader.ReadAsync(request.DocumentStream, cancellationToken);
            var document = UniDocument.FromContent(content, request.DocumentName);
            var fingerprint = await _fingerprintsProvider.GetForDocumentAsync(document, cancellationToken);
            return OperationResult.Successful(fingerprint);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, CalculateFingerprintDocumentInternalError);
            return OperationResult.Failed<TextFingerprint>(CalculateFingerprintDocumentInternalError, e.Message);
        }
    }
}