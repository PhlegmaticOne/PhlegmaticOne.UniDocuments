using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.Text.Domain.Services.Reports.Models;

public class ReportParagraphsData
{
    public string Content { get; }
    public List<ReportParagraphData> ParagraphsData { get; }

    public ReportParagraphsData(string content)
    {
        Content = content;
        ParagraphsData = new List<ReportParagraphData>();
    }

    public void AddParagraphData(Guid documentId, string documentName, int localId, CompareTextResult compareTextResult)
    {
        var data = ReportParagraphData
            .WithDocumentData(documentId, documentName, localId)
            .FillFromCompareResult(compareTextResult);
        
        ParagraphsData.Add(data);   
    }
}