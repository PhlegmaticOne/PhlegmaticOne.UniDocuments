using UniDocuments.Text.Domain.Services.Common;
using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsNeuralDataHandler
{
    Task LoadAsync();
    ParagraphSaveData GetSaveData(int id);
    void OnTrainDataSetup(RawDocument document, RawParagraph paragraph);
    void OnTrainComplete();
}