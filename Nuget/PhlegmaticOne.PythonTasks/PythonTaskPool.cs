using System.Collections.Concurrent;
using Python.Runtime;

namespace PhlegmaticOne.PythonTasks;

public class PythonTaskPool : IPythonTaskPool
{
    private static PythonTaskPool? Instance;
    
    private readonly IPythonScriptNamesProvider _scriptNamesProvider;
    private readonly BlockingCollection<PythonTask> _tasks;
    
    public PythonTaskPool(IPythonScriptNamesProvider scriptNamesProvider)
    {
        _scriptNamesProvider = scriptNamesProvider;
        _tasks = new BlockingCollection<PythonTask>();
        Instance = this;
    }

    public static PythonTaskPool CreateAndStart(
        IPythonScriptNamesProvider scriptNamesProvider,
        CancellationToken cancellationToken)
    {
        if (Instance is not null)
        {
            return Instance;
        }
        
        Instance = new PythonTaskPool(scriptNamesProvider);
        Instance.Start(cancellationToken);
        PythonTask.TaskPool = Instance;
        return Instance;
    }

    public void Start(CancellationToken cancellationToken)
    {
        PythonTask.TaskPool = this;
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

                dynamic method = script.GetAttr(task.MethodName);
                var result = method(task.Input);
                task.OnComplete(result);
            }
        }
        catch (OperationCanceledException)
        {
            //Python thread pool running cancelled
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