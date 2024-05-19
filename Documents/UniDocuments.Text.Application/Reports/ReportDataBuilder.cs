using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Providers.Reports.Builder;
using UniDocuments.Text.Domain.Services.Reports;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Application.Reports;

public class ReportDataBuilder : IReportDataBuilder
{
    private readonly ITextCompareProvider _textCompareProvider;
    private readonly IReportDataLoader _reportDataLoader;
    private readonly IFingerprintsProvider _fingerprintsProvider;

    public ReportDataBuilder(
        ITextCompareProvider textCompareProvider,
        IReportDataLoader reportDataLoader,
        IFingerprintsProvider fingerprintsProvider)
    {
        _textCompareProvider = textCompareProvider;
        _reportDataLoader = reportDataLoader;
        _fingerprintsProvider = fingerprintsProvider;
    }
    
    public async Task<ReportData> BuildReportDataAsync(
        ReportDataBuildRequest reportDataBuildRequest, CancellationToken cancellationToken)
    {
        var data = await _reportDataLoader.LoadAsync(reportDataBuildRequest, cancellationToken).ConfigureAwait(false);
        var sourceData = GetSourceData(reportDataBuildRequest, data);

        var documentsData = Task.Run(() => BuildDocumentData(data, sourceData), cancellationToken);
        var paragraphsData = Task.Run(() => BuildParagraphsData(reportDataBuildRequest, data), cancellationToken);

        await Task.WhenAll(documentsData, paragraphsData);
        
        return new ReportData(sourceData.ToDocumentData(0), documentsData.Result, paragraphsData.Result);
    }

    private List<ReportDocumentData> BuildDocumentData(Dictionary<Guid, ReportLoadData> documentData, ReportLoadData sourceData)
    {
        var result = new List<ReportDocumentData>();

        foreach (var data in documentData)
        {
            if (data.Key == sourceData.Id)
            {
                continue;
            }

            var compareResult = _fingerprintsProvider.Compare(sourceData.Fingerprint, data.Value.Fingerprint);

            if (compareResult.IsSuspicious)
            {
                result.Add(data.Value.ToDocumentData(compareResult.Similarity));
            }
        }

        return result;
    }

    private List<ReportParagraphsData> BuildParagraphsData(
        ReportDataBuildRequest buildRequest, Dictionary<Guid, ReportLoadData> documentData)
    {
        var result = new List<ReportParagraphsData>();
        var paragraphsCount = buildRequest.Document.Content.Paragraphs.Count;

        for (var i = 0; i < paragraphsCount; i++)
        {
            var paragraphsData = BuildReportParagraphsData(buildRequest, documentData, i);

            if (paragraphsData.ParagraphsData.Count > 0)
            {
                result.Add(paragraphsData);
            }
        }

        return result;
    }

    private ReportLoadData GetSourceData(ReportDataBuildRequest request, Dictionary<Guid, ReportLoadData> loaded)
    {
        if (request.Document.Id != Guid.Empty)
        {
            return loaded[request.Document.Id];
        }

        return new ReportLoadData
        {
            Id = Guid.Empty,
            DateLoaded = DateTime.UtcNow,
            StudentFirstName = string.Empty,
            StudentLastName = string.Empty,
            Name = string.Empty,
            Document = request.Document,
            Fingerprint = _fingerprintsProvider.Get(request.Document)
        };
    }

    private ReportParagraphsData BuildReportParagraphsData(
        ReportDataBuildRequest buildRequest, Dictionary<Guid, ReportLoadData> documentData, int index)
    {
        var metric = buildRequest.BaseMetric;
        var sourceParagraphs = buildRequest.Document.Content.Paragraphs;
        var sourceParagraph = sourceParagraphs[index];
        var suspiciousParagraphs = buildRequest.PlagiarismSearchResponse.SuspiciousParagraphs[index];
        var paragraphsData = new ReportParagraphsData(sourceParagraph);
            
        foreach (var suspiciousSearchData in suspiciousParagraphs.SuspiciousParagraphs)
        {
            if (suspiciousSearchData.DocumentId == Guid.Empty)
            {
                continue;
            }

            var suspiciousDocument = documentData[suspiciousSearchData.DocumentId];
            var suspiciousParagraph = suspiciousDocument.Document.GetParagraph(suspiciousSearchData.Id);
            var compareResult = _textCompareProvider.Compare(sourceParagraph, suspiciousParagraph, metric);

            if (compareResult.IsSuspicious)
            {
                var documentDataMap = suspiciousDocument.ToDocumentData(compareResult.SimilarityValue);
                paragraphsData.AddParagraphData(new ReportParagraphData(suspiciousParagraph, documentDataMap));
            }
        }

        return paragraphsData;
    }
}