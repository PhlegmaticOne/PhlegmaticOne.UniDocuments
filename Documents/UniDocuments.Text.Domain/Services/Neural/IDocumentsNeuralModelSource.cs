using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralModelSource
{
    Task<DocumentNeuralTrainData> GetTrainDataAsync(int documentNumber);
}