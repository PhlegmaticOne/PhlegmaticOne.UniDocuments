namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;

public class PlagiarismSearchAlgorithmData
{
    public PlagiarismSearchAlgorithmData(bool useFingerprint, string modelName)
    {
        UseFingerprint = useFingerprint;
        ModelName = modelName;
    }

    public bool UseFingerprint { get; }
    public string ModelName { get; }
}