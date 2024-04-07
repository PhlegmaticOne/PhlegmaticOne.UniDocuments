using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Services.Neural.Services;

namespace UniDocuments.Text.Services.Neural;

public static class DocumentsNeuralRegistration
{
    public static IServiceCollection AddNeural(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDocumentsNeuralModel, DocumentNeuralModelService>();
        serviceCollection.AddSingleton<IDocumentsNeuralModelSource, DocumentNeuralModelSourceFile>();
        return serviceCollection;
    }
 }