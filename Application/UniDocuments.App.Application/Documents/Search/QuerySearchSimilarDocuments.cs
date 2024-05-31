using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Documents;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Documents.Search;

public class QuerySearchSimilarDocuments : IOperationResultQuery<DocumentsSearchResult>
{
    public int Count { get; set; }
    public string Phrase { get; set; } = null!;
    public string ModelName { get; set; } = null!;

    public PlagiarismSearchRequest ToPlagiarismSearchRequest()
    {
        var document = UniDocument.FromString(Phrase);
        return new PlagiarismSearchRequest(document, Count, 0, ModelName);
    }
}

public class QuerySearchSimilarDocumentsHandler : 
    IOperationResultQueryHandler<QuerySearchSimilarDocuments, DocumentsSearchResult>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly ApplicationDbContext _dbContext;

    public QuerySearchSimilarDocumentsHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        ApplicationDbContext dbContext)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<DocumentsSearchResult>> Handle(
        QuerySearchSimilarDocuments request, CancellationToken cancellationToken)
    {
        var searchRequest = request.ToPlagiarismSearchRequest();
        var response = await _plagiarismSearchProvider.SearchAsync(searchRequest);

        return OperationResult.Successful(new DocumentsSearchResult
        {
            Entries = await SelectEntries(response.SuspiciousParagraphs[0], cancellationToken)
        });
    }

    private Task<List<DocumentSearchResultEntry>> SelectEntries(
        ParagraphPlagiarismData plagiarismData, CancellationToken cancellationToken)
    {
        var documentIds = plagiarismData.SelectUniqueIds();

        return _dbContext.Set<StudyDocument>()
            .Where(x => documentIds.Contains(x.Id))
            .Select(x => new DocumentSearchResultEntry
            {
                DocumentId = x.Id,
                DateLoaded = x.DateLoaded,
                DocumentName = x.Name,
                StudentFirstName = x.Student.FirstName,
                StudentLastName = x.Student.LastName
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }
}