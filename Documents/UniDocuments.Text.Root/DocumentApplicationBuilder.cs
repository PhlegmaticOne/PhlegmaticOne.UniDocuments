using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Algorithms;
using UniDocuments.Text.Domain.Features.Factories;
using UniDocuments.Text.Domain.Features.Providers;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.Similarity;
using UniDocuments.Text.Domain.Services.DocumentNameMapping;
using UniDocuments.Text.Domain.Services.Documents;
using UniDocuments.Text.Domain.Services.FileStorage;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint;
using UniDocuments.Text.Features.Text;
using UniDocuments.Text.Features.TextVector;
using UniDocuments.Text.Root.Builders;
using UniDocuments.Text.Root.Builders.Features;

namespace UniDocuments.Text.Root;

public class DocumentApplicationBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentApplicationBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        _serviceCollection.AddSingleton<IUniDocumentFeatureProvider, UniDocumentFeatureProvider>();
    }

    public DocumentApplicationBuilder UseAlgorithm<T>() where T : class, IPlagiarismAlgorithm
    {
        _serviceCollection.AddSingleton<IPlagiarismAlgorithm, T>();
        return this;
    }

    public void UseFingerprintFeature(Action<FingerprintFeatureInstallBuilder> action)
    {
        var builder = new FingerprintFeatureInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IUniDocumentFeatureFactory, UniDocumentFeatureFingerprintFactory>();
        action(builder);
    }

    public void UseTextVectorFeature()
    {
        _serviceCollection.AddSingleton<IUniDocumentSharedFeatureFactory, UniDocumentFeatureTextVectorFactory>();
    }
    
    public void UseTextFeature(Action<TextFeatureInstallBuilder> action)
    {
        var builder = new TextFeatureInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IUniDocumentFeatureFactory, UniDocumentFeatureTextFactory>();
        action(builder);
    }
        
    public DocumentApplicationBuilder UseSharedFeature<T>() where T : class, IUniDocumentSharedFeatureFactory
    {
        _serviceCollection.AddSingleton<IUniDocumentSharedFeatureFactory, T>();
        return this;
    }

    public void UseDocumentNameMapper<TDev, TProd>(bool isDevelopment) 
        where TDev : class, IDocumentToNameMapper
        where TProd : class, IDocumentToNameMapper
    {
        if (isDevelopment)
        {
            _serviceCollection.AddSingleton<IDocumentToNameMapper, TDev>();
        }
        else
        {
            _serviceCollection.AddSingleton<IDocumentToNameMapper, TProd>();
        }
    }
        
    public void UseDocumentsService<T>(Action<DocumentServiceInstallBuilder> action) where T : class, IUniDocumentsService
    {
        var builder = new DocumentServiceInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IUniDocumentsService, T>();
        action(builder);
    }
        
    public void UseFileStorage<TDev, TProd>(bool isDevelopment, Action<FileStorageInstallBuilder> action) 
        where TDev : class, IFileStorage, IFileStorageIndexable
        where TProd : class, IFileStorage
    {
        if (isDevelopment)
        {
            _serviceCollection.AddSingleton<TDev>();
            _serviceCollection.AddSingleton<IFileStorage>(x => x.GetRequiredService<TDev>());
            _serviceCollection.AddSingleton<IFileStorageIndexable>(x => x.GetRequiredService<TDev>());
        }
        else
        {
            var builder = new FileStorageInstallBuilder(_serviceCollection);
            _serviceCollection.AddSingleton<IFileStorage, TProd>();
            action(builder);
        }
    }

    public void UseSavePathProvider<T>() where T : class, ISavePathProvider
    {
        _serviceCollection.AddSingleton<ISavePathProvider, T>();
    }
        
    public void UseStreamContentReader<T>() where T : class, IStreamContentReader
    {
        _serviceCollection.AddSingleton<IStreamContentReader, T>();
    }
        
    public void UseTextPreprocessor<T>(Action<TextPreprocessorInstallBuilder> builderAction) 
        where T : class, ITextPreprocessor
    {
        var builder = new TextPreprocessorInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<ITextPreprocessor, T>();
        builderAction(builder);
    }

    public void UseNeuralModel<T>(Action<DocumentsNeuralInstallBuilder> action) where T : class, IDocumentsNeuralModel
    {
        var builder = new DocumentsNeuralInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IDocumentsNeuralModel, T>();
        action(builder);
    }
    
    public void UseSimilarityFinder<T>() where T : class, IDocumentsSimilarityFinder
    {
        _serviceCollection.AddSingleton<IDocumentsSimilarityFinder, T>();
    }
    
    public void UsePlagiarismFinder<T>() where T : class, IPlagiarismFinder
    {
        _serviceCollection.AddSingleton<IPlagiarismFinder, T>();
    }
}