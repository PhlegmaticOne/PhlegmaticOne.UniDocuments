using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Providers.Reports.Data;
using UniDocuments.Text.Domain.Providers.Reports.Data.Models;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.Text.Application.Reports;

public class PlagiarismReportDataBuilder : IPlagiarismReportDataBuilder
{
    private readonly IDocumentMapper _documentMapper;
    private readonly ITextCompareProvider _textCompareProvider;
    private readonly IParagraphGlobalReader _paragraphGlobalReader;

    public PlagiarismReportDataBuilder(
        IDocumentMapper documentMapper,
        ITextCompareProvider textCompareProvider,
        IParagraphGlobalReader paragraphGlobalReader)
    {
        _documentMapper = documentMapper;
        _textCompareProvider = textCompareProvider;
        _paragraphGlobalReader = paragraphGlobalReader;
    }
    
    public async Task<PlagiarismReportData> BuildReportDataAsync(PlagiarismReportDataRequest reportDataRequest, CancellationToken cancellationToken)
    {
        var response = reportDataRequest.PlagiarismSearchResponse;
        var documentsData = BuildDocumentData(reportDataRequest);
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

    private List<ReportDocumentData> BuildDocumentData(PlagiarismReportDataRequest request)
    {
        return request.PlagiarismSearchResponse.SuspiciousDocuments.Select(x =>
        {
            var name = _documentMapper.GetDocumentData(x.Id)!.Name;
            return new ReportDocumentData(x.Id, name, x.Similarity);
        }).ToList();
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

            if (compareResult.IsNoSimilar())
            {
                continue;
            }
            
            var documentName = _documentMapper.GetDocumentData(documentId)!.Name;
            data.AddParagraphData(documentId, documentName, suspiciousParagraph.Id, compareResult);
        }

        return data;
    }
    
    private Task<string[]> LoadSuspiciousParagraphs(ParagraphPlagiarismData data, CancellationToken token)
    {
        var suspiciousData = data.SuspiciousParagraphs;
        var suspiciousParagraphsLoad = suspiciousData.Select(x => _paragraphGlobalReader.ReadAsync(x.Id, token));
        return Task.WhenAll(suspiciousParagraphsLoad);
    }
}