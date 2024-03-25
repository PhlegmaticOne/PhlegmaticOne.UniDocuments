using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Plagiarism.Winnowing.Algorithm.Hash;

namespace UniDocuments.Text.Plagiarism.Winnowing.Algorithm;

public static class TextWinnowingInstaller
{
    public class TextWinnowingBuilder
    {
        private readonly IServiceCollection _serviceCollection;

        public TextWinnowingBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void UseHashAlgorithm<T>() where T : class, IFingerprintHash
        {
            _serviceCollection.AddSingleton<IFingerprintHash, T>();
        }
    }
    
    public static IServiceCollection AddTextWinnowing(this IServiceCollection serviceCollection, 
        Action<TextWinnowingBuilder> builderAction)
    {
        var builder = new TextWinnowingBuilder(serviceCollection);
        serviceCollection.AddSingleton<ITextWinnowing, TextWinnowing>();
        builderAction(builder);
        return serviceCollection;
    }
}