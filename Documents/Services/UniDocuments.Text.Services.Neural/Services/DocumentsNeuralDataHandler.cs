using Newtonsoft.Json;
using UniDocuments.Text.Domain.Services.Common;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.SavePath;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentsNeuralDataHandler : IDocumentsNeuralDataHandler
{
    private readonly ISavePathProvider _savePathProvider;

    private Dictionary<int, ParagraphSaveData> _paragraphsToDocumentsMap;

    public DocumentsNeuralDataHandler(ISavePathProvider savePathProvider)
    {
        _savePathProvider = savePathProvider;
        _paragraphsToDocumentsMap = new Dictionary<int, ParagraphSaveData>();
    }

    public async Task LoadAsync()
    {
        var json = await File.ReadAllTextAsync(_savePathProvider.SavePath);
        _paragraphsToDocumentsMap = JsonConvert.DeserializeObject<Dictionary<int, ParagraphSaveData>>(json)!;
    }

    public ParagraphSaveData GetSaveData(int id)
    {
        return _paragraphsToDocumentsMap[id];
    }

    public void OnTrainDataSetup(RawDocument document, RawParagraph paragraph)
    {
        _paragraphsToDocumentsMap.TryAdd(paragraph.Id, new ParagraphSaveData
        {
            DocumentId = document.Id,
            OriginalId = paragraph.OriginalId
        });
    }

    public void OnTrainComplete()
    {
        var path = _savePathProvider.SavePath;
        var json = JsonConvert.SerializeObject(_paragraphsToDocumentsMap);
        File.WriteAllText(path, json);
    }
}