using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Features.Fingerprint.Services;
using UniDocuments.Text.Features.Text.Contracts;

namespace UniDocuments.Text.Root.Builders.Features;

public class FingerprintFeatureInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public FingerprintFeatureInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
        
    public void UseFingerprintContainer<T>() where T : class, IFingerprintsContainer
    {
        _serviceCollection.AddSingleton<IFingerprintsContainer, T>();
    }
    
    public void UseFingerprintComputer<T>() where T : class, IFingerprintComputer
    {
        _serviceCollection.AddSingleton<IFingerprintComputer, T>();
    }
    
    public void UseFingerprintHash<T>() where T : class, IFingerprintHash
    {
        _serviceCollection.AddSingleton<IFingerprintHash, T>();
    }
    
    public void UseFingerprintSearcher<T>() where T : class, IFingerprintSearcher
    {
        _serviceCollection.AddSingleton<IFingerprintSearcher, T>();
    }
    
    public void UseFingerprintAlgorithm<T>() where T : class, IFingerprintAlgorithm
    {
        _serviceCollection.AddSingleton<IFingerprintAlgorithm, T>();
    }
}