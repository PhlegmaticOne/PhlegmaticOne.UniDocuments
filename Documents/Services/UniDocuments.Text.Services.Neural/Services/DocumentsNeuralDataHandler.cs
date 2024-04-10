using Newtonsoft.Json;
using UniDocuments.Text.Domain.Services.Common;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.SavePath;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentsNeuralDataHandler : IDocumentsNeuralDataHandler
{
    private const string SaveFileName = "documents_mapping.json";
    
    private readonly ISavePathProvider _savePathProvider;
    private readonly IDocumentsMapper _documentsMapper;

    private Dictionary<int, ParagraphSaveData> _paragraphsToDocumentsMap;

    public DocumentsNeuralDataHandler(ISavePathProvider savePathProvider, IDocumentsMapper documentsMapper)
    {
        _savePathProvider = savePathProvider;
        _documentsMapper = documentsMapper;
        _paragraphsToDocumentsMap = new Dictionary<int, ParagraphSaveData>();
    }

    public async Task LoadAsync()
    {
        var json = await File.ReadAllTextAsync(_savePathProvider.SavePath);
        _paragraphsToDocumentsMap = JsonConvert.DeserializeObject<Dictionary<int, ParagraphSaveData>>(json)!;
    }

    public ParagraphSaveData GetSaveData(int id)
    {
        var data = _paragraphsToDocumentsMap[id];
        data.DocumentName = _documentsMapper.GetDocumentName(data.DocumentId);
        return data;
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
        var path = Path.Combine(_savePathProvider.SavePath, SaveFileName);
        var json = JsonConvert.SerializeObject(_paragraphsToDocumentsMap);
        File.WriteAllText(path, json);
    }
}