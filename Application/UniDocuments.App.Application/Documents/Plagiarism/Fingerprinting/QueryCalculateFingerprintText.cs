using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;

namespace UniDocuments.App.Application.Documents.Plagiarism.Fingerprinting;

public class QueryCalculateFingerprintText : IOperationResultQuery<TextFingerprint>
{
    public string Text { get; set; } = null!;
}

public class QueryCalculateFingerprintTextHandler :
    IOperationResultQueryHandler<QueryCalculateFingerprintText, TextFingerprint>
{
    private const string CalculateFingerprintTextInternalError = "CalculateFingerprintText.InternalError";
    
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly ILogger<QueryCalculateFingerprintTextHandler> _logger;

    public QueryCalculateFingerprintTextHandler(
        IFingerprintsProvider fingerprintsProvider,
        ILogger<QueryCalculateFingerprintTextHandler> logger)
    {
        _fingerprintsProvider = fingerprintsProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<TextFingerprint>> Handle(QueryCalculateFingerprintText request, CancellationToken cancellationToken)
    {
        try
        {
            var document = UniDocument.FromString(request.Text);
            var result = await _fingerprintsProvider.GetForDocumentAsync(document, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, CalculateFingerprintTextInternalError);
            return OperationResult.Failed<TextFingerprint>(CalculateFingerprintTextInternalError, e.Message);
        }
    }
}