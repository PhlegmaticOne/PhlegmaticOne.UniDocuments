using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Algorithms;
using UniDocuments.Text.Domain.Features.Factories;
using UniDocuments.Text.Domain.Features.Providers;
using UniDocuments.Text.Domain.Services;
using UniDocuments.Text.Domain.Services.Preprocessing;

namespace UniDocuments.Text.Application;

public static class DocumentsInstaller
{
    public class DocumentAlgorithmsBuilder
    {
        private readonly IServiceCollection _serviceCollection;

        public DocumentAlgorithmsBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            _serviceCollection.AddSingleton<IUniDocumentFeatureProvider, UniDocumentFeatureProvider>();
        }

        public DocumentAlgorithmsBuilder UseAlgorithm<T>() where T : class, IPlagiarismAlgorithm
        {
            _serviceCollection.AddSingleton<IPlagiarismAlgorithm, T>();
            return this;
        }

        public DocumentAlgorithmsBuilder UseService<T, TImpl>() 
            where T : class, IDocumentService
            where TImpl : class, T
        {
            _serviceCollection.AddSingleton<T, TImpl>();
            return this;
        }

        public DocumentAlgorithmsBuilder UseFeature<T>() where T : class, IUniDocumentFeatureFactory
        {
            _serviceCollection.AddSingleton<IUniDocumentFeatureFactory, T>();
            return this;
        }
        
        public DocumentAlgorithmsBuilder UseSharedFeature<T>() where T : class, IUniDocumentSharedFeatureFactory
        {
            _serviceCollection.AddSingleton<IUniDocumentSharedFeatureFactory, T>();
            return this;
        }
    }
    
    public static IServiceCollection AddDocumentAlgorithms(
        this IServiceCollection serviceCollection,
        Action<DocumentAlgorithmsBuilder> builderAction)
    {
        var builder = new DocumentAlgorithmsBuilder(serviceCollection);
        builderAction(builder);
        return serviceCollection;
    }
}