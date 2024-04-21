using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsTrainDatasetSource : IDisposable
{
    void Initialize();
    Task<DocumentTrainModel> GetNextDocumentAsync();
    Task<List<DocumentTrainModel>> GetBatchAsync(object paragraphIdsObject);
}