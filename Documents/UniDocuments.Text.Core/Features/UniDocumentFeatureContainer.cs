namespace UniDocuments.Text.Core.Features;

public class UniDocumentFeatureContainer
{
    private List<Task<IUniDocumentFeature>>? _taskFeatures;
    
    public UniDocumentFeatureBelongType BelongType { get; }
    public IUniDocumentFeature[]? CreatedFeatures { get; private set; }

    public bool HasCreatedFeatures => CreatedFeatures is not null;

    public static UniDocumentFeatureContainer Original => new(UniDocumentFeatureBelongType.Original);
    public static UniDocumentFeatureContainer Comparing => new(UniDocumentFeatureBelongType.Comparing);
    public static UniDocumentFeatureContainer Shared => new(UniDocumentFeatureBelongType.Shared);

    private UniDocumentFeatureContainer(UniDocumentFeatureBelongType belongType) => BelongType = belongType;

    public void AddTaskFeature(Task<IUniDocumentFeature> taskFeature)
    {
        _taskFeatures ??= new List<Task<IUniDocumentFeature>>();
        _taskFeatures.Add(taskFeature);
    }

    public async Task CreateFeatures()
    {
        if (_taskFeatures is null)
        {
            return;
        }
        
        CreatedFeatures = await Task.WhenAll(_taskFeatures);
    }
}