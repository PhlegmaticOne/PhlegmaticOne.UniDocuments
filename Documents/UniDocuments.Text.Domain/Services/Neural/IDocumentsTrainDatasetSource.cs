using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsTrainDatasetSource
{
    Task<DocumentTrainModel> GetDocumentAsync(int id);
    Task<List<DocumentTrainModel>> GetBatchAsync(object paragraphIdsObject);
}