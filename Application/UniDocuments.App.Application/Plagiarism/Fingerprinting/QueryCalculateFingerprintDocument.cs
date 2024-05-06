using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.App.Application.Plagiarism.Fingerprinting;

public class QueryCalculateFingerprintDocument : IOperationResultQuery<TextFingerprint>
{
    public Guid Id { get; set; }
}

public class QueryCalculateFingerprintDocumentHandler : 
    IOperationResultQueryHandler<QueryCalculateFingerprintDocument, TextFingerprint>
{
    private const string? CalculateFingerprintDocumentInternalError = "CalculateFingerprintDocument.InternalError";
    
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly ILogger<QueryCalculateFingerprintDocumentHandler> _logger;

    public QueryCalculateFingerprintDocumentHandler(
        IFingerprintsProvider fingerprintsProvider,
        ILogger<QueryCalculateFingerprintDocumentHandler> logger)
    {
        _fingerprintsProvider = fingerprintsProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<TextFingerprint>> Handle(QueryCalculateFingerprintDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var fingerprint = await _fingerprintsProvider.GetForDocumentAsync(request.Id, cancellationToken);
            return OperationResult.Successful(fingerprint);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, CalculateFingerprintDocumentInternalError);
            return OperationResult.Failed<TextFingerprint>(CalculateFingerprintDocumentInternalError, e.Message);
        }
    }
}