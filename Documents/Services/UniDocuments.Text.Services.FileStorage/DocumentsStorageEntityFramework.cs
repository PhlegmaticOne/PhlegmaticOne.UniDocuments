using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Extensions;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Services.FileStorage;

public class DocumentsStorageEntityFramework : IDocumentsStorage
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentsStorageEntityFramework(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IDocumentLoadResponse?> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<StudyDocumentFile>()
            .Where(x => x.Id == id)
            .Select(x => new DocumentLoadResponseFromBytes
            {
                Id = id,
                Name = x.Name,
                Bytes = x.Content
            })
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public IAsyncEnumerable<IDocumentLoadResponse> LoadAsync(IList<Guid> ids)
    {
        return _dbContext.Set<StudyDocumentFile>()
            .Where(x => ids.Contains(x.Id))
            .Select(x => new DocumentLoadResponseFromBytes
            {
                Id = x.Id,
                Bytes = x.Content,
                Name = x.Name
            })
            .AsAsyncEnumerable();
    }

    public async Task<Guid> SaveAsync(StorageSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        var set = _dbContext.Set<StudyDocumentFile>();
        saveRequest.Stream.SeekToZero();
        
        await using var memoryStream = new MemoryStream();
        await saveRequest.Stream.CopyToAsync(memoryStream, cancellationToken);

        var file = await set.FirstOrDefaultAsync(x => x.Id == saveRequest.Id, cancellationToken);

        if (file is not null)
        {
            return UpdateExisting(file, memoryStream, saveRequest);
        }

        return await CreateNew(memoryStream, saveRequest, cancellationToken);
    }

    private async Task<Guid> CreateNew(
        MemoryStream memoryStream, StorageSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        var entry = await _dbContext.Set<StudyDocumentFile>().AddAsync(new StudyDocumentFile
        {
            Id = saveRequest.Id,
            Content = memoryStream.ToArray(),
            Name = saveRequest.Name
        }, cancellationToken);
        
        return entry.Entity.Id;
    }

    private Guid UpdateExisting(
        StudyDocumentFile file, MemoryStream memoryStream, StorageSaveRequest saveRequest)
    {
        file.Content = memoryStream.ToArray();
        file.Name = saveRequest.Name;
        _dbContext.Update(file);
        return file.Id;
    }
}