using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Providers.Reports.Builder;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Extensions;
using UniDocuments.Text.Domain.Services.Fingerprinting;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Application.Reports;

public class ReportDataBuilder : IReportDataBuilder
{
    private readonly IDocumentMapper _documentMapper;
    private readonly ITextCompareProvider _textCompareProvider;
    private readonly IParagraphGlobalReader _paragraphGlobalReader;
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly IFingerprintsComparer _fingerprintsComparer;

    public ReportDataBuilder(
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
    
    public async Task<ReportData> BuildReportDataAsync(
        ReportDataBuildRequest reportDataBuildRequest, CancellationToken cancellationToken)
    {
        var response = reportDataBuildRequest.PlagiarismSearchResponse;
        var documentsData = await BuildDocumentData(reportDataBuildRequest, cancellationToken);
        var paragraphsData = await BuildParagraphsData(reportDataBuildRequest, cancellationToken);
        return new ReportData(response.DocumentId, response.DocumentName, documentsData, paragraphsData);
    }

    private async Task<List<ReportParagraphsData>> BuildParagraphsData(
        ReportDataBuildRequest buildRequest, CancellationToken cancellationToken)
    {
        var result = new List<ReportParagraphsData>();
        var paragraphs = buildRequest.Document.Content.Paragraphs;

        for (var i = 0; i < paragraphs.Count; i++)
        {
            var paragraph = paragraphs[i];
            var suspicious = buildRequest.PlagiarismSearchResponse.SuspiciousParagraphs[i];
            var suspiciousParagraphs = await LoadSuspiciousParagraphs(suspicious, cancellationToken);

            if (suspiciousParagraphs.Length == 0)
            {
                continue;
            }
            
            var compareRequest = new CompareTextsRequest(paragraph, suspiciousParagraphs, buildRequest.BaseMetric);
            var compareResponse = await _textCompareProvider.CompareAsync(compareRequest, cancellationToken);
            var reportData = MapToReportParagraphData(paragraph, suspicious, compareResponse);
            result.Add(reportData);
        }

        return result;
    }

    private async Task<List<ReportDocumentData>> BuildDocumentData(
        ReportDataBuildRequest buildRequest, CancellationToken cancellationToken)
    {
        var sourceFingerprint = await _fingerprintsProvider.GetForDocumentAsync(buildRequest.Document, cancellationToken);
        var fingerprints = await LoadSuspiciousDocumentFingerprint(buildRequest, cancellationToken);
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
        ReportDataBuildRequest buildRequest, CancellationToken cancellationToken)
    {
        var loadFingerprints = new HashSet<Guid>();

        foreach (var suspiciousParagraph in buildRequest.PlagiarismSearchResponse.SuspiciousParagraphs)
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
        
        var suspiciousParagraphsLoad = suspiciousData
            .Where(x => x.DocumentId != Guid.Empty)
            .Select(x => _paragraphGlobalReader.ReadAsync(x.Id, token));
        
        return Task.WhenAll(suspiciousParagraphsLoad);
    }
}