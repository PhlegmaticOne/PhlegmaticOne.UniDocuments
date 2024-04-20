using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Shared;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralDataHandler
{
    Task LoadAsync(CancellationToken cancellationToken);
    ParagraphNeuralSaveData GetSaveData(int id);
    void OnTrainDataSetup(RawDocument document, RawParagraph paragraph);
    void OnTrainComplete();
}