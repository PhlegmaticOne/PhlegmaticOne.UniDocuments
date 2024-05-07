using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Preprocessing.Stemming;
using UniDocuments.Text.Domain.Services.Preprocessing.Stopwords;

namespace UniDocuments.Text.Root.Builders;

public class TextPreprocessorInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public TextPreprocessorInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseStopWordsService<T>() where T : class, IStopWordsService
    {
        _serviceCollection.AddSingleton<IStopWordsService, T>();
    }
        
    public void UseStopWordsLoader<T>() where T : class, IStopWordsLoader
    {
        _serviceCollection.AddSingleton<IStopWordsLoader, T>();
    }

    public void UseStemmer<T>() where T : class, IStemmer
    {
        _serviceCollection.AddSingleton<IStemmer, T>();
    }
}