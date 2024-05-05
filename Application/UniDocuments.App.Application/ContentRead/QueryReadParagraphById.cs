using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.ContentReading;

namespace UniDocuments.App.Application.ContentRead;

public class QueryReadParagraphById : IOperationResultQuery<string>
{
    public int ParagraphId { get; set; }
}

public class QueryReadParagraphByIdHandler : IOperationResultQueryHandler<QueryReadParagraphById, string>
{
    private readonly IParagraphGlobalReader _paragraphGlobalReader;

    public QueryReadParagraphByIdHandler(IParagraphGlobalReader paragraphGlobalReader)
    {
        _paragraphGlobalReader = paragraphGlobalReader;
    }
    
    public async Task<OperationResult<string>> Handle(
        QueryReadParagraphById request, CancellationToken cancellationToken)
    {
        var paragraph = await _paragraphGlobalReader.ReadAsync(request.ParagraphId, cancellationToken);

        return string.IsNullOrEmpty(paragraph) ?
            OperationResult.Failed<string>("ReadParagraphById.ParagraphNotExist") :
            OperationResult.Successful(paragraph);
    }
}