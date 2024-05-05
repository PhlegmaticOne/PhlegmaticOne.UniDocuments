using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Extensions;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Services.FileStorage.EntityFramework;

public class DocumentsStorageEntityFramework : IDocumentsStorage
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentsStorageEntityFramework(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<DocumentLoadResponse> LoadAsync(DocumentLoadRequest loadRequest, CancellationToken cancellationToken)
    {
        var document = await _dbContext.Set<StudyDocumentFile>()
            .FirstOrDefaultAsync(x => x.StudyDocumentId == loadRequest.Id, cancellationToken: cancellationToken);

        var stream = new MemoryStream(document!.Content);
        return new DocumentLoadResponse(document.Name, stream);
    }

    public async Task<Guid> SaveAsync(DocumentSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        saveRequest.Stream.SeekToZero();
        
        await using var memoryStream = new MemoryStream();
        await saveRequest.Stream.CopyToAsync(memoryStream, cancellationToken);
        
        var entry = await _dbContext.Set<StudyDocumentFile>().AddAsync(new StudyDocumentFile
        {
            StudyDocumentId = saveRequest.Id,
            Content = memoryStream.ToArray(),
            Name = saveRequest.Name
        }, cancellationToken);
        
        return entry.Entity.Id;
    }
}