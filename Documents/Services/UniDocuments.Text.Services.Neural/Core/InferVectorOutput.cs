namespace UniDocuments.Text.Services.Neural.Core;

public class InferVectorOutput
{
    public InferVectorOutput(int paragraphId, List<InferVectorEntry> inferEntries)
    {
        ParagraphId = paragraphId;
        InferEntries = inferEntries;
    }

    public int ParagraphId { get; }
    public List<InferVectorEntry> InferEntries { get; }
}