using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.App.Application.Plagiarism;

public class QueryCalculateFingerprintDocument : IOperationResultQuery<TextFingerprint>
{
    public Guid Id { get; set; }
}

public class QueryCalculateFingerprintDocumentHandler : 
    IOperationResultQueryHandler<QueryCalculateFingerprintDocument, TextFingerprint>
{
    private readonly IFingerprintContainer _fingerprintContainer;

    public QueryCalculateFingerprintDocumentHandler(IFingerprintContainer fingerprintContainer)
    {
        _fingerprintContainer = fingerprintContainer;
    }
    
    public Task<OperationResult<TextFingerprint>> Handle(QueryCalculateFingerprintDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var fingerprint = _fingerprintContainer.Get(request.Id);
            var result = fingerprint is null
                ? OperationResult.Failed<TextFingerprint>("CalculateFingerprint.NotFound")
                : OperationResult.Successful(fingerprint);
            return Task.FromResult(result);
        }
        catch (Exception e)
        {
            var result = OperationResult.Failed<TextFingerprint>("CalculateFingerprint.InternalError", e.Message);
            return Task.FromResult(result);
        }
    }
}