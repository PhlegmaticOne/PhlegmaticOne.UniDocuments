using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentNeuralModelSourceFile : IDocumentsNeuralModelSource
{
    private const int Count = 100;
    private const string BasePath = @"C:\Users\lolol\Downloads\t\{0}.txt";
    
    public async Task<DocumentNeuralTrainData> GetTrainDataAsync(int documentNumber)
    {
        if (documentNumber >= Count)
        {
            return DocumentNeuralTrainData.NoData();
        }

        var filePath = string.Format(BasePath, documentNumber);
        var content = await File.ReadAllTextAsync(filePath);
        return new DocumentNeuralTrainData(Guid.NewGuid(), new List<DocumentNeuralParagraph>
        {
            new(0, content)
        });
    }
}