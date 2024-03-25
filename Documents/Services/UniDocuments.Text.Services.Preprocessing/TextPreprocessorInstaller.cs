using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Preprocessing;

namespace UniDocuments.Text.Services.Preprocessing;

public static class TextPreprocessorInstaller
{
    public class TextPreprocessorBuilder
    {
        private readonly IServiceCollection _serviceCollection;

        public TextPreprocessorBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void UseStemmer<T>() where T : class, IStemmer
        {
            _serviceCollection.AddSingleton<IStemmer, T>();
        }
    }
    
    public static IServiceCollection AddTextPreprocessor(this IServiceCollection serviceCollection,
        Action<TextPreprocessorBuilder> builderAction)
    {
        var builder = new TextPreprocessorBuilder(serviceCollection);
        serviceCollection.AddSingleton<ITextPreprocessor, TextPreprocessor>();
        builderAction(builder);
        return serviceCollection;
    }
}