﻿using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Application.PlagiarismSearching;

public class PlagiarismSearchProvider : IPlagiarismSearchProvider
{
    private readonly INeuralNetworkPlagiarismSearcher _networkPlagiarismSearcher;

    public PlagiarismSearchProvider(INeuralNetworkPlagiarismSearcher networkPlagiarismSearcher)
    {
        _networkPlagiarismSearcher = networkPlagiarismSearcher;
    }
    
    public async Task<PlagiarismSearchResponse> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var response = new PlagiarismSearchResponse(request.Document.Id)
        {
            SuspiciousParagraphs = await _networkPlagiarismSearcher.SearchAsync(request, cancellationToken)
        };

        return response;
    }
}