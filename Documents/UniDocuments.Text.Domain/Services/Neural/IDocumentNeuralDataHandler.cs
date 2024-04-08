using UniDocuments.Text.Domain.Services.Common;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentNeuralDataHandler
{
    void OnTrainDataSetup(RawDocument document, RawParagraph paragraph);
}