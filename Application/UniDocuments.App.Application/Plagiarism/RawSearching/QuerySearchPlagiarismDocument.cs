using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Application.Plagiarism.RawSearching.Base;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Plagiarism.RawSearching;

public class QuerySearchPlagiarismDocument : QuerySearchPlagiarism
{
    public Stream FileStream { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class QuerySearchPlagiarismDocumentHandler :
    IOperationResultQueryHandler<QuerySearchPlagiarismDocument, PlagiarismSearchResponseDocument>
{
    private const string SearchPlagiarismDocumentInternalError = "SearchPlagiarismDocument.InternalError";
    
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IStreamContentReader _streamContentReader;
    private readonly ILogger<QuerySearchPlagiarismDocumentHandler> _logger;

    public QuerySearchPlagiarismDocumentHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IStreamContentReader streamContentReader,
        ILogger<QuerySearchPlagiarismDocumentHandler> logger)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _streamContentReader = streamContentReader;
        _logger = logger;
    }

    public async Task<OperationResult<PlagiarismSearchResponseDocument>> Handle(
        QuerySearchPlagiarismDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var content = await _streamContentReader.ReadAsync(request.FileStream, cancellationToken);
            var document = UniDocument.FromContent(content, request.Name);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopCount, request.InferEpochs, request.ModelName); 
            var result = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, SearchPlagiarismDocumentInternalError);
            return OperationResult
                .Failed<PlagiarismSearchResponseDocument>(SearchPlagiarismDocumentInternalError, e.Message);
        }
    }
}