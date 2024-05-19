using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Providers.Fingerprinting;
using UniDocuments.Text.Domain.Providers.Reports.Builder;
using UniDocuments.Text.Domain.Services.Reports;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Application.Reports;

public class ReportDataLoader : IReportDataLoader
{
    private readonly IDocumentLoadingProvider _documentsProvider;
    private readonly IFingerprintsProvider _fingerprintsProvider;
    private readonly ApplicationDbContext _dbContext;

    public ReportDataLoader(
        IDocumentLoadingProvider documentsProvider,
        IFingerprintsProvider fingerprintsProvider,
        ApplicationDbContext dbContext)
    {
        _documentsProvider = documentsProvider;
        _fingerprintsProvider = fingerprintsProvider;
        _dbContext = dbContext;
    }
    
    public async Task<Dictionary<Guid, ReportLoadData>> LoadAsync(
        ReportDataBuildRequest request, CancellationToken cancellationToken)
    {
        var result = new Dictionary<Guid, ReportLoadData>();
        var includedDocuments = GetIncludedDocuments(request);
        var fingerprints = await _fingerprintsProvider.GetForDocumentsAsync(includedDocuments, cancellationToken);
        var documents = await _documentsProvider.LoadAsync(includedDocuments, true, cancellationToken);

        await foreach (var documentData in LoadDocumentsData(includedDocuments, cancellationToken))
        {
            documentData.Document = documents[documentData.Id];
            documentData.Fingerprint = fingerprints[documentData.Id];
            result.Add(documentData.Id, documentData);
        }

        return result;
    }
    
    private ConfiguredCancelableAsyncEnumerable<ReportLoadData> LoadDocumentsData(
        HashSet<Guid> documents, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StudyDocument>()
            .Where(x => documents.Contains(x.Id))
            .Select(x => new ReportLoadData
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