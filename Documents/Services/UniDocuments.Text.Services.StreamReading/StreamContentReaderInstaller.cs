using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Services.StreamReading;

public static class StreamContentReaderInstaller
{
    public static IServiceCollection AddStreamContentReader(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IStreamContentReader, StreamContentReaderWordDocument>();
        return serviceCollection;
    }
}