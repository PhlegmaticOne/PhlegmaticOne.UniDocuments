using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.App.Application.Plagiarism.Fingerprinting;

public class QueryCalculateFingerprintText : IOperationResultQuery<TextFingerprint>
{
    public string Text { get; set; } = null!;
}

public class QueryCalculateFingerprintTextHandler :
    IOperationResultQueryHandler<QueryCalculateFingerprintText, TextFingerprint>
{
    private readonly IFingerprintAlgorithm _fingerprintAlgorithm;
    private readonly IFingerprintOptionsProvider _optionsProvider;

    public QueryCalculateFingerprintTextHandler(
        IFingerprintAlgorithm fingerprintAlgorithm, 
        IFingerprintOptionsProvider optionsProvider)
    {
        _fingerprintAlgorithm = fingerprintAlgorithm;
        _optionsProvider = optionsProvider;
    }
    
    public async Task<OperationResult<TextFingerprint>> Handle(QueryCalculateFingerprintText request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Task.Run(() => Fingerprint(request.Text), cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult.Failed<TextFingerprint>("CalculateFingerprint.InternalError", e.Message);
        }
    }

    private TextFingerprint Fingerprint(string text)
    {
        var options = _optionsProvider.GetOptions();
        var content = UniDocument.FromString(text);
        return _fingerprintAlgorithm.Fingerprint(content, options);
    }
}