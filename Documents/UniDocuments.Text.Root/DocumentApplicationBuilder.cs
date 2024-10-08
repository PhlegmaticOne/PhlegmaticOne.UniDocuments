﻿using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Providers.BaseMetrics;
using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.Reports.Provider;
using UniDocuments.Text.Domain.Services.Cache;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Preprocessing.Preprocessor;
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

    public void UseDocumentMapper<T>(Action<DocumentMapperInstallBuilder> builderAction) 
        where T : class, IDocumentMapper
    {
        var builder = new DocumentMapperInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IDocumentMapper, T>();
        builderAction(builder);
    }
        
    public void UseDocumentsCache<T>() where T : class, IUniDocumentsCache
    {
        _serviceCollection.AddSingleton<IUniDocumentsCache, T>();
    }
        
    public void UseDocumentStorage<T>(Action<DocumentStorageInstallBuilder> builderAction) 
        where T : class, IDocumentsStorage
    {
        var builder = new DocumentStorageInstallBuilder(_serviceCollection);
        _serviceCollection.AddScoped<IDocumentsStorage, T>();
        builderAction(builder);
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
        
    public void UseStreamContentReader<T>(Action<StreamContentReaderInstallBuilder> builderAction) 
        where T : class, IStreamContentReader
    {
        var builder = new StreamContentReaderInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IStreamContentReader, T>();
        builderAction(builder);
    }
        
    public void UseTextPreprocessor<T>(Action<TextPreprocessorInstallBuilder> builderAction) 
        where T : class, ITextPreprocessor
    {
        var builder = new TextPreprocessorInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<ITextPreprocessor, T>();
        builderAction(builder);
    }

    public void UseNeuralModelProvider<T>(Action<DocumentNeuralInstallBuilder> action)
        where T : class, IDocumentNeuralModelsProvider
    {
        var builder = new DocumentNeuralInstallBuilder(_serviceCollection);
        _serviceCollection.AddSingleton<IDocumentNeuralModelsProvider, T>();
        action(builder);
    }
    
    public void UseSimilarityService<T>() where T : class, ITextCompareProvider
    {
        _serviceCollection.AddScoped<ITextCompareProvider, T>();
    }
    
    public void UsePlagiarismSearcher<T>() where T : class, IPlagiarismSearchProvider
    {
        _serviceCollection.AddSingleton<IPlagiarismSearchProvider, T>();
    }
    
    public void UseDocumentLoadingProvider<T>() where T : class, IDocumentLoadingProvider
    {
        _serviceCollection.AddScoped<IDocumentLoadingProvider, T>();
    }

    public void UseReportProvider<T>(Action<ReportInstallBuilder> builderAction)
        where T : class, IReportProvider
    {
        _serviceCollection.AddScoped<IReportProvider, T>();
        var builder = new ReportInstallBuilder(_serviceCollection);
        builderAction(builder);
    }
}