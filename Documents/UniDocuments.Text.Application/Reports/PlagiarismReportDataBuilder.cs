using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Providers.Reports.Data;
using UniDocuments.Text.Domain.Providers.Reports.Data.Models;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;

namespace UniDocuments.Text.Application.Reports;

public class PlagiarismReportDataBuilder : IPlagiarismReportDataBuilder
{
    private readonly IDocumentMapper _documentMapper;
    private readonly ITextCompareProvider _textCompareProvider;
    private readonly IParagraphGlobalReader _paragraphGlobalReader;
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly IFingerprintsComparer _fingerprintsComparer;

    public PlagiarismReportDataBuilder(
        IDocumentMapper documentMapper,
        ITextCompareProvider textCompareProvider,
        IParagraphGlobalReader paragraphGlobalReader,
        IFingerprintsProvider fingerprintsProvider,
        IFingerprintsComparer fingerprintsComparer)
    {
        _documentMapper = documentMapper;
        _textCompareProvider = textCompareProvider;
        _paragraphGlobalReader = paragraphGlobalReader;
        _fingerprintsProvider = fingerprintsProvider;
        _fingerprintsComparer = fingerprintsComparer;
    }
    
    public async Task<PlagiarismReportData> BuildReportDataAsync(PlagiarismReportDataRequest reportDataRequest, CancellationToken cancellationToken)
    {
        var response = reportDataRequest.PlagiarismSearchResponse;
        var documentsData = await BuildDocumentData(reportDataRequest, cancellationToken);
        var paragraphsData = await BuildParagraphsData(reportDataRequest, cancellationToken);
        return new PlagiarismReportData(response.DocumentId, response.DocumentName, documentsData, paragraphsData);
    }

    private async Task<List<ReportParagraphsData>> BuildParagraphsData(
        PlagiarismReportDataRequest request, CancellationToken cancellationToken)
    {
        var result = new List<ReportParagraphsData>();
        var paragraphs = request.Document.Content!.Paragraphs;

        for (var i = 0; i < paragraphs.Count; i++)
        {
            var paragraph = paragraphs[i];
            var suspicious = request.PlagiarismSearchResponse.SuspiciousParagraphs[i];
            var suspiciousParagraphs = await LoadSuspiciousParagraphs(suspicious, cancellationToken);
            var compareRequest = new CompareTextsRequest(paragraph, suspiciousParagraphs, request.BaseMetric);
            var compareResponse = await _textCompareProvider.CompareAsync(compareRequest, cancellationToken);
            var reportData = MapToReportParagraphData(paragraph, suspicious, compareResponse);
            result.Add(reportData);
        }

        return result;
    }

    private async Task<List<ReportDocumentData>> BuildDocumentData(
        PlagiarismReportDataRequest request, CancellationToken cancellationToken)
    {
        var sourceFingerprint = await _fingerprintsProvider.GetForDocumentAsync(request.Document, cancellationToken);
        var fingerprints = await LoadSuspiciousDocumentFingerprint(request, cancellationToken);
        return _fingerprintsComparer.Compare(sourceFingerprint, fingerprints);
    }

    private ReportParagraphsData MapToReportParagraphData(
        string paragraph, ParagraphPlagiarismData plagiarismData, CompareTextsResponse compareTextsResponse)
    {
        var data = new ReportParagraphsData(paragraph);

        for (var i = 0; i < plagiarismData.SuspiciousParagraphs.Count; i++)
        {
            var suspiciousParagraph = plagiarismData.SuspiciousParagraphs[i];
            var documentId = suspiciousParagraph.DocumentId;
            var compareResult = compareTextsResponse.SimilarityResults[i];

            if (compareResult.IsSuspicious == false)
            {
                continue;
            }
            
            var documentName = _documentMapper.GetDocumentData(documentId)!.Name;
            data.AddParagraphData(documentId, documentName, suspiciousParagraph.Id, compareResult);
        }

        return data;
    }

    private Task<List<TextFingerprint>> LoadSuspiciousDocumentFingerprint(
        PlagiarismReportDataRequest request, CancellationToken cancellationToken)
    {
        var loadFingerprints = new HashSet<Guid>();

        foreach (var suspiciousParagraph in request.PlagiarismSearchResponse.SuspiciousParagraphs)
        {
            foreach (var paragraph in suspiciousParagraph.SuspiciousParagraphs)
            {
                loadFingerprints.Add(paragraph.DocumentId);
            }
        }

        return _fingerprintsProvider.GetForDocumentsAsync(loadFingerprints, cancellationToken);
    }
    
    private Task<string[]> LoadSuspiciousParagraphs(ParagraphPlagiarismData data, CancellationToken token)
    {
        var suspiciousData = data.SuspiciousParagraphs;
        var suspiciousParagraphsLoad = suspiciousData.Select(x => _paragraphGlobalReader.ReadAsync(x.Id, token));
        return Task.WhenAll(suspiciousParagraphsLoad);
    }
}