namespace PhlegmaticOne.PythonTasks;

internal class PythonScriptNamesProviderInternal : IPythonScriptNamesProvider
{
    private readonly string[] _scriptNames;

    public PythonScriptNamesProviderInternal(params string[] scriptNames)
    {
        _scriptNames = scriptNames;
    }
    
    public IEnumerable<string> GetScriptNames()
    {
        return _scriptNames;
    }
}