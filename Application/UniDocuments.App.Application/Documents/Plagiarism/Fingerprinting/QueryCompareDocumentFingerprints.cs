using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Extensions;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Documents.Plagiarism.Fingerprinting;

public class QueryCompareDocumentFingerprints : IOperationResultQuery<double>
{
    public Stream Original { get; set; } = null!;
    public string OriginalName { get; set; } = null!;
    public Stream Suspicious { get; set; } = null!;
    public string SuspiciousName { get; set; } = null!;
}

public class QueryCompareDocumentFingerprintsHandler : 
    IOperationResultQueryHandler<QueryCompareDocumentFingerprints, double>
{
    private const string ErrorMessage = "CompareDocumentFingerprints.InternalError";
    
    private readonly IStreamContentReader _contentReader;
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly IFingerprintsComparer _fingerprintsComparer;
    private readonly ILogger<QueryCompareDocumentFingerprintsHandler> _logger;

    public QueryCompareDocumentFingerprintsHandler(
        IStreamContentReader contentReader,
        IFingerprintsProvider fingerprintsProvider,
        IFingerprintsComparer fingerprintsComparer,
        ILogger<QueryCompareDocumentFingerprintsHandler> logger)
    {
        _contentReader = contentReader;
        _fingerprintsProvider = fingerprintsProvider;
        _fingerprintsComparer = fingerprintsComparer;
        _logger = logger;
    }
    
    public async Task<OperationResult<double>> Handle(
        QueryCompareDocumentFingerprints request, CancellationToken cancellationToken)
    {
        try
        {
            var source = await CreateFingerprint(request.Original, request.OriginalName, cancellationToken);
            var suspicious = await CreateFingerprint(request.Suspicious, request.SuspiciousName, cancellationToken);

            var result = _fingerprintsComparer.Compare(source, suspicious.ToList());
            return OperationResult.Successful(result[0].Similarity);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<double>(ErrorMessage, e.Message);
        }
    }

    private async Task<TextFingerprint> CreateFingerprint(Stream stream, string name, CancellationToken cancellationToken)
    {
        var content = await _contentReader.ReadAsync(stream, cancellationToken);
        var document = UniDocument.FromContent(content, name);
        return await _fingerprintsProvider.ComputeAsync(document, cancellationToken);
    }
}