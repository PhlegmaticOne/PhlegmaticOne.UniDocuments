using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Matching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Services.BaseMetrics.Provider;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Documents;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Root.Builders;

namespace UniDocuments.Text.Root;

public class DocumentApplicationBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentApplicationBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseBaseMetrics<T>(Action<TextBaseMetricsInstallBuilder> builderAction)
        where T : class, ITextSimilarityBaseMetricsProvider
    {
        var builder = new TextBaseMetricsInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<ITextSimilarityBaseMetricsProvider, T>();
        builderAction(builder);
    }

    public void UseDocumentMapper<T>(Action<DocumentMapperInstallBuilder> builderAction) where T : class, IDocumentMapper
    {
        var builder = new DocumentMapperInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IDocumentMapper, T>();
        builderAction(builder);
    }
        
    public void UseDocumentsService<T>(Action<DocumentServiceInstallBuilder> action) where T : class, IUniDocumentsService
    {
        var builder = new DocumentServiceInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IUniDocumentsService, T>();
        action(builder);
    }
        
    public void UseFileStorage<TDev, TProd>(bool isDevelopment, Action<DocumentStorageInstallBuilder> action) 
        where TDev : class, IDocumentsStorage, IDocumentStorageIndexable
        where TProd : class, IDocumentsStorage
    {
        if (isDevelopment)
        {
            _serviceCollection.AddSingleton<TDev>();
            _serviceCollection.AddSingleton<IDocumentsStorage>(x => x.GetRequiredService<TDev>());
            _serviceCollection.AddSingleton<IDocumentStorageIndexable>(x => x.GetRequiredService<TDev>());
        }
        else
        {
            var builder = new DocumentStorageInstallBuilder(_serviceCollection);
            _serviceCollection.AddSingleton<IDocumentsStorage, TProd>();
            action(builder);
        }
    }

    public void UseFingerprint(Action<FingerprintingInstallBuilder> builderAction)
    {
        var builder = new FingerprintingInstallBuilder(_serviceCollection);
        builderAction(builder);
    }

    public void UseSavePathProvider<T>() where T : class, ISavePathProvider
    {
        _serviceCollection.AddSingleton<ISavePathProvider, T>();
    }
        
    public void UseStreamContentReader<T>(Action<StreamContentReaderInstallBuilder> builderAction) where T : class, IStreamContentReader
    {
        var builder = new StreamContentReaderInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IStreamContentReader, T>();
        builderAction(builder);
    }
        
    public void UseTextPreprocessor<T>(Action<TextPreprocessorInstallBuilder> builderAction) where T : class, ITextPreprocessor
    {
        var builder = new TextPreprocessorInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<ITextPreprocessor, T>();
        builderAction(builder);
    }

    public void UseNeuralModel<T>(Action<DocumentNeuralInstallBuilder> action) where T : class, IDocumentsNeuralModel
    {
        var builder = new DocumentNeuralInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IDocumentsNeuralModel, T>();
        action(builder);
    }

    public void UseMatchingService<T>(Action<MatchingInstallBuilder> builderAction) where T : class, ITextMatchProvider
    {
        var builder = new MatchingInstallBuilder(_serviceCollection);
        _serviceCollection.AddScoped<ITextMatchProvider, T>();
        builderAction(builder);
    }
    
    public void UseSimilarityService<T>() where T : class, ITextCompareProvider
    {
        _serviceCollection.AddScoped<ITextCompareProvider, T>();
    }
    
    public void UsePlagiarismSearcher<T>() where T : class, IPlagiarismSearcher
    {
        _serviceCollection.AddSingleton<IPlagiarismSearcher, T>();
    }
}