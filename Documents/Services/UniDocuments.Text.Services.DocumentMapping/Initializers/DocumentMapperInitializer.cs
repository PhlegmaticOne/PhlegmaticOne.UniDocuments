using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.Text.Services.DocumentMapping.Initializers;

public class DocumentMapperInitializer : IDocumentMapperInitializer
{
    private readonly IDocumentMapper _documentMapper;
    private readonly ApplicationDbContext _dbContext;

    public DocumentMapperInitializer(IDocumentMapper documentMapper, ApplicationDbContext dbContext)
    {
        _documentMapper = documentMapper;
        _dbContext = dbContext;
    }
    
    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        var query = _dbContext.Set<StudyDocument>().Select(x => new 
        {
            x.Id,
            x.Name
        });

        foreach (var documentData in query)
        {
            _documentMapper.AddMap(documentData.Id, documentData.Name);
        }
        
        return Task.CompletedTask;
    }
}