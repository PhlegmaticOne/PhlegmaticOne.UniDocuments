namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;

public class PlagiarismSearchAlgorithmData
{
    public PlagiarismSearchAlgorithmData(bool useFingerprint, string neuralModelType)
    {
        UseFingerprint = useFingerprint;
        NeuralModelType = neuralModelType;
    }

    public bool UseFingerprint { get; }
    public string NeuralModelType { get; }
}