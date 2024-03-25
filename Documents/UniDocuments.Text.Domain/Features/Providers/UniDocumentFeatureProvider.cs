using UniDocuments.Text.Domain.Features.Factories;

namespace UniDocuments.Text.Domain.Features.Providers;

public class UniDocumentFeatureProvider : IUniDocumentFeatureProvider
{
    private readonly Dictionary<UniDocumentFeatureFlag, IUniDocumentFeatureFactory> _featureFactories;
    private readonly Dictionary<UniDocumentFeatureFlag, IUniDocumentSharedFeatureFactory> _sharedFeatureFactories;
    
    public UniDocumentFeatureProvider(
        IEnumerable<IUniDocumentFeatureFactory> featureFactories,
        IEnumerable<IUniDocumentSharedFeatureFactory> sharedFeatureFactories)
    {
        _featureFactories = featureFactories.ToDictionary(x => x.FeatureFlag, x => x);
        _sharedFeatureFactories = sharedFeatureFactories.ToDictionary(x => x.FeatureFlag, x => x);
    }
    
    public async Task SetupFeatures(IEnumerable<UniDocumentFeatureFlag> featureFlags, UniDocumentEntry entry, CancellationToken cancellationToken)
    {
        var grouped = featureFlags.GroupBy(x => x.SetupOrder).OrderBy(x => x.Key);

        foreach (var flagsGroup in grouped)
        {
            var originalContainer = UniDocumentFeatureContainer.Original;
            var comparingContainer = UniDocumentFeatureContainer.Comparing;
            var sharedContainer = UniDocumentFeatureContainer.Shared;

            foreach (var featureFlag in flagsGroup)
            {
                if (featureFlag.IsShared)
                {
                    var setupTask = _sharedFeatureFactories[featureFlag].CreateFeature(entry, cancellationToken);
                    sharedContainer.AddTaskFeature(setupTask);
                    continue;
                }

                EnsureFeatureExists(entry.Comparing, featureFlag, comparingContainer, cancellationToken);
                EnsureFeatureExists(entry.Original, featureFlag, originalContainer, cancellationToken);
            }

            await Task.WhenAll(
                originalContainer.CreateFeatures(),
                comparingContainer.CreateFeatures(),
                sharedContainer.CreateFeatures());

            entry.TakeFeatures(originalContainer);
            entry.TakeFeatures(comparingContainer);
            entry.TakeFeatures(sharedContainer);
        }
    }

    private void EnsureFeatureExists(
        UniDocument document, UniDocumentFeatureFlag featureFlag, UniDocumentFeatureContainer featureContainer,
        CancellationToken cancellationToken)
    {
        if (document.ContainsFeature(featureFlag) == false)
        {
            var setupTask = _featureFactories[featureFlag].CreateFeature(document, cancellationToken);
            featureContainer.AddTaskFeature(setupTask);
        }
    }
}