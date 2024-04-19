namespace PhlegmaticOne.PythonTasks;

public interface IPythonScriptNamesProvider
{
    IEnumerable<string> GetScriptNames();
}