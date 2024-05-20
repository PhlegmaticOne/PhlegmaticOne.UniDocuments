using System.Collections.Concurrent;
using Python.Runtime;

namespace PhlegmaticOne.PythonTasks;

public class PythonTaskPool : IPythonTaskPool
{
    private readonly IPythonScriptNamesProvider _scriptNamesProvider;
    private readonly BlockingCollection<PythonTask> _tasks;

    public static PythonTaskPool CreateAndStart(
        IPythonScriptNamesProvider scriptNamesProvider,
        CancellationToken cancellationToken)
    {
        var result = new PythonTaskPool(scriptNamesProvider);
        result.Start(cancellationToken);
        return result;
    }

    public PythonTaskPool(IPythonScriptNamesProvider scriptNamesProvider)
    {
        _scriptNamesProvider = scriptNamesProvider;
        _tasks = new BlockingCollection<PythonTask>();
    }

    public int QueueCount => _tasks.Count;

    public void Start(CancellationToken cancellationToken)
    {
        Task.Factory.StartNew(() => PythonExecutionThread(cancellationToken), TaskCreationOptions.LongRunning);
    }

    public void Enqueue(PythonTask task)
    {
        _tasks.Add(task);
    }

    private void PythonExecutionThread(CancellationToken cancellationToken)
    {
        PythonEngine.Initialize();
        var scripts = ImportScripts();

        try
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                var task = _tasks.Take(cancellationToken);

                if (!scripts.TryGetValue(task.ScriptName, out var script))
                {
                    continue;
                }

                try
                {
                    dynamic method = script.GetAttr(task.MethodName);
                    var result = method(task.Input);
                    task.OnComplete(result);
                }
                catch
                {
                    //Unexpected error
                }
            }
        }
        catch (OperationCanceledException)
        {
            //Python thread pool running cancelled
        }
        catch
        {
            //Unexpected error
        }
        
        PythonEngine.Shutdown();
    }

    private Dictionary<string, PyObject> ImportScripts()
    {
        return _scriptNamesProvider
            .GetScriptNames()
            .ToDictionary(x => x, PythonEngine.ImportModule);
    }
}