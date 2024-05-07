using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;

namespace UniDocuments.App.Application.Plagiarism.Fingerprinting;

public class QueryCalculateFingerprintExistingDocument : IOperationResultQuery<TextFingerprint>
{
    public Guid Id { get; set; }
}

public class QueryCalculateFingerprintExistingDocumentHandler : 
    IOperationResultQueryHandler<QueryCalculateFingerprintExistingDocument, TextFingerprint>
{
    private const string? CalculateFingerprintDocumentInternalError = "CalculateFingerprintExistingDocument.InternalError";
    
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly ILogger<QueryCalculateFingerprintExistingDocumentHandler> _logger;

    public QueryCalculateFingerprintExistingDocumentHandler(
        IFingerprintsProvider fingerprintsProvider,
        ILogger<QueryCalculateFingerprintExistingDocumentHandler> logger)
    {
        _fingerprintsProvider = fingerprintsProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<TextFingerprint>> Handle(QueryCalculateFingerprintExistingDocument request, CancellationToken cancellationToken)
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