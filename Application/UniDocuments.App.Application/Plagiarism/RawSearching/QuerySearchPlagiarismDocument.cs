using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Plagiarism.RawSearching;

public class QuerySearchPlagiarismDocument : IOperationResultQuery<PlagiarismSearchResponseDocument>
{
    public QuerySearchPlagiarismDocument(Stream fileStream, int topN, string modelName)
    {
        FileStream = fileStream;
        TopN = topN;
        ModelName = modelName;
    }

    public Stream FileStream { get; }
    public int TopN { get; }
    public string ModelName { get; }
}

public class QuerySearchPlagiarismDocumentHandler :
    IOperationResultQueryHandler<QuerySearchPlagiarismDocument, PlagiarismSearchResponseDocument>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IStreamContentReader _streamContentReader;

    public QuerySearchPlagiarismDocumentHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IStreamContentReader streamContentReader)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _streamContentReader = streamContentReader;
    }

    public async Task<OperationResult<PlagiarismSearchResponseDocument>> Handle(
        QuerySearchPlagiarismDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var content = await _streamContentReader.ReadAsync(request.FileStream, cancellationToken);
            var document = UniDocument.FromContent(content);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopN, request.ModelName); 
            var result = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult
                .Failed<PlagiarismSearchResponseDocument>("SearchPlagiarismDocument.InternalError", e.Message);
        }
    }
}