﻿using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism.RawSearching;

public class QuerySearchPlagiarismText : IOperationResultQuery<PlagiarismSearchResponseText>
{
    public string Text { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public int TopN { get; set; }
}

public class QuerySearchPlagiarismTextHandler : 
    IOperationResultQueryHandler<QuerySearchPlagiarismText, PlagiarismSearchResponseText>
{
    private const string SearchPlagiarismTextInternalError = "SearchPlagiarismText.InternalError";
    
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly ILogger<QuerySearchPlagiarismTextHandler> _logger;

    public QuerySearchPlagiarismTextHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        ILogger<QuerySearchPlagiarismTextHandler> logger)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponseText>> Handle(
        QuerySearchPlagiarismText request, CancellationToken cancellationToken)
    {
        try
        {
            var document = UniDocument.FromString(request.Text);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopN, request.ModelName);
            var topParagraphs = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            var result = new PlagiarismSearchResponseText(topParagraphs.SuspiciousParagraphs);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, SearchPlagiarismTextInternalError);
            return OperationResult.Failed<PlagiarismSearchResponseText>(SearchPlagiarismTextInternalError, e.Message);
        }
    }
}