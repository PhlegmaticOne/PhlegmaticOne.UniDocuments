using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism.RawSearching.Base;

public class QuerySearchPlagiarism : IOperationResultQuery<PlagiarismSearchResponseDocument>
{
    public int TopCount { get; set; }
    public string ModelName { get; set; } = null!;
}