namespace UniDocuments.Text.Services.Neural.Core;

public class InferVectorEntry
{
    public int ParagraphId { get; }
    public double Similarity { get; }

    public InferVectorEntry(int paragraphId, double similarity)
    {
        ParagraphId = paragraphId;
        Similarity = similarity;
    }
}