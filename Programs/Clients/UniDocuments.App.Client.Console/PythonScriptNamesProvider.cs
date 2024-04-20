using PhlegmaticOne.PythonTasks;

namespace UniDocuments.App.Client.Console;

public class PythonScriptNamesProvider : IPythonScriptNamesProvider
{
    public IEnumerable<string> GetScriptNames()
    {
        yield return @"test\divider";
    }
}