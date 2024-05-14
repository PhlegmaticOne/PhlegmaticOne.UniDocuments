using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.Text.Services.DocumentMapping.Initializers;

public class DocumentMappingInitializer : IDocumentMappingInitializer
{
    private readonly IDocumentMapper _documentMapper;
    private readonly ApplicationDbContext _dbContext;

    public DocumentMappingInitializer(IDocumentMapper documentMapper, ApplicationDbContext dbContext)
    {
        _documentMapper = documentMapper;
        _dbContext = dbContext;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var query = _dbContext
            .Set<StudyDocument>()
            .Select(x => new 
            { 
                x.Id, 
                x.ValuableParagraphsCount
            })
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken);

        await foreach (var data in query)
        {
            _documentMapper.AddDocument(data.Id, data.ValuableParagraphsCount);
        }
    }
}