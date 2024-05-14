using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Providers.Reports.Builder;
using UniDocuments.Text.Domain.Services.Fingerprinting.Models;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Application.Reports;

public class ReportDataBuilder : IReportDataBuilder
{
    private class DocumentData
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateLoaded { get; set; }
        public string StudentFirstName { get; set; } = null!;
        public string StudentLastName { get; set; } = null!;
        public TextFingerprint Fingerprint { get; set; } = null!;
        public UniDocument Document { get; set; } = null!;

        public ReportDocumentData Map(double similarity)
        {
            return new ReportDocumentData
            {
                Id = Id,
                DateLoaded = DateLoaded,
                StudentFirstName = StudentFirstName,
                StudentLastName = StudentLastName,
                Name = Name,
                Similarity = similarity
            };
        }
    }
    
    private readonly ITextCompareProvider _textCompareProvider;
    private readonly IDocumentsProvider _documentsProvider;
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly ApplicationDbContext _dbContext;

    public ReportDataBuilder(
        ITextCompareProvider textCompareProvider,
        IDocumentsProvider documentsProvider,
        IFingerprintsProvider fingerprintsProvider,
        ApplicationDbContext dbContext)
    {
        _textCompareProvider = textCompareProvider;
        _documentsProvider = documentsProvider;
        _fingerprintsProvider = fingerprintsProvider;
        _dbContext = dbContext;
    }
    
    public async Task<ReportData> BuildReportDataAsync(
        ReportDataBuildRequest reportDataBuildRequest, CancellationToken cancellationToken)
    {
        var data = await GetDataAsync(reportDataBuildRequest, cancellationToken).ConfigureAwait(false);
        var sourceData = GetSourceData(reportDataBuildRequest, data);

        var documentsData = await Task.Run(() => BuildDocumentData(data, sourceData), cancellationToken);
        var paragraphsData = await Task.Run(() => BuildParagraphsData(reportDataBuildRequest, data), cancellationToken);

        //await Task.WhenAll(documentsData, paragraphsData);
        
        return new ReportData(sourceData.Map(0), documentsData, paragraphsData);
    }

    private static List<ReportDocumentData> BuildDocumentData(Dictionary<Guid, DocumentData> documentData, DocumentData sourceData)
    {
        var result = new List<ReportDocumentData>();

        foreach (var data in documentData)
        {
            if (data.Key == sourceData.Id)
            {
                continue;
            }

            var similarity = sourceData.Fingerprint.CalculateJaccard(data.Value.Fingerprint);
            result.Add(data.Value.Map(similarity));
        }

        return result;
    }

    private List<ReportParagraphsData> BuildParagraphsData(
        ReportDataBuildRequest buildRequest, Dictionary<Guid, DocumentData> documentData)
    {
        var result = new List<ReportParagraphsData>();
        var paragraphs = buildRequest.Document.Content.Paragraphs;

        for (var i = 0; i < paragraphs.Count; i++)
        {
            var paragraph = paragraphs[i];
            var suspicious = buildRequest.PlagiarismSearchResponse.SuspiciousParagraphs[i];
            var paragraphsData = new ReportParagraphsData(paragraph);
            
            foreach (var suspiciousParagraph in suspicious.SuspiciousParagraphs)
            {
                if (suspiciousParagraph.DocumentId == Guid.Empty)
                {
                    continue;
                }

                var paragraphData = new ReportParagraphData
                {
                    Content = paragraph
                };
                
                var suspiciousData = documentData[suspiciousParagraph.DocumentId];
                var suspiciousDataParagraph = suspiciousData.Document.Content.Paragraphs[suspiciousParagraph.Id];
                var compareResult = _textCompareProvider.Compare(paragraph, suspiciousDataParagraph, buildRequest.BaseMetric);

                if (compareResult.IsSuspicious)
                {
                    paragraphData.DocumentData = suspiciousData.Map(compareResult.SimilarityValue);
                    paragraphsData.ParagraphsData.Add(paragraphData);
                }
            }

            if (paragraphsData.ParagraphsData.Count > 0)
            {
                result.Add(paragraphsData);
            }
        }

        return result;
    }

    private DocumentData GetSourceData(ReportDataBuildRequest request, Dictionary<Guid, DocumentData> loaded)
    {
        if (request.Document.Id != Guid.Empty)
        {
            return loaded[request.Document.Id];
        }

        return new DocumentData
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

    private async Task<Dictionary<Guid, DocumentData>> GetDataAsync(ReportDataBuildRequest request, CancellationToken cancellationToken)
    {
        var includedDocuments = GetIncludedDocuments(request);

        var fingerprintsTask = await LoadFingerprints(includedDocuments, cancellationToken);
        var documentsContentTask = await LoadDocumentsContent(includedDocuments, cancellationToken);
        
        //await Task.WhenAll(fingerprintsTask, documentsContentTask);

        var fingerprints = fingerprintsTask;
        var documentContents = documentsContentTask;

        var result = new Dictionary<Guid, DocumentData>();
        
        await foreach (var documentData in LoadDocumentsData(includedDocuments, cancellationToken))
        {
            documentData.Document = documentContents[documentData.Id];
            documentData.Fingerprint = fingerprints[documentData.Id];
            result.Add(documentData.Id, documentData);
        }

        return result;
    }

    private ConfiguredCancelableAsyncEnumerable<DocumentData> LoadDocumentsData(
        HashSet<Guid> documents, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StudyDocument>()
            .Where(x => documents.Contains(x.Id))
            .Select(x => new DocumentData
            {
                Id = x.Id,
                Name = x.Name,
                DateLoaded = x.DateLoaded,
                StudentFirstName = x.Student.FirstName,
                StudentLastName = x.Student.LastName,
            })
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false);
    }

    private Task<Dictionary<Guid, TextFingerprint>> LoadFingerprints(HashSet<Guid> documents, CancellationToken cancellationToken)
    {
        return _fingerprintsProvider.GetForDocumentsAsync(documents, cancellationToken);
    }

    private Task<Dictionary<Guid, UniDocument>> LoadDocumentsContent(HashSet<Guid> documents, CancellationToken cancellationToken)
    {
        return _documentsProvider.GetDocumentsAsync(documents, cancellationToken);
    }

    private static HashSet<Guid> GetIncludedDocuments(ReportDataBuildRequest request)
    {
        var includedDocuments = new HashSet<Guid>();

        if (request.Document.Id != Guid.Empty)
        {
            includedDocuments.Add(request.Document.Id);
        }

        foreach (var paragraph in request.PlagiarismSearchResponse.SuspiciousParagraphs)
        {
            foreach (var suspiciousParagraph in paragraph.SuspiciousParagraphs)
            {
                includedDocuments.Add(suspiciousParagraph.DocumentId);
            }
        }

        return includedDocuments;
    }
}