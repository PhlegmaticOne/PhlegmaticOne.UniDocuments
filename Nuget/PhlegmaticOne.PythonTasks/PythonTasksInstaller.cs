using Microsoft.Extensions.DependencyInjection;

namespace PhlegmaticOne.PythonTasks;

public static class PythonTasksInstaller
{
    public static IServiceCollection AddPythonTaskPool(this IServiceCollection serviceCollection, params string[] scriptNames)
    {
        serviceCollection.AddSingleton<IPythonScriptNamesProvider>(_ => new PythonScriptNamesProviderInternal(scriptNames));
        serviceCollection.AddSingleton<IPythonTaskPool, PythonTaskPool>();
        return serviceCollection;
    }
}