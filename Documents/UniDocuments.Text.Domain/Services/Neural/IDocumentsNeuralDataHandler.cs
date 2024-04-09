using UniDocuments.Text.Domain.Services.Common;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralDataHandler
{
    Task LoadAsync();
    ParagraphSaveData GetSaveData(int id);
    void OnTrainDataSetup(RawDocument document, RawParagraph paragraph);
    void OnTrainComplete();
}